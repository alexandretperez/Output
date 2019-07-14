using Output.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Output
{
    public sealed class Mapper : IMapper
    {
        private readonly IDictionary<int, Delegate> _compiledCache = new Dictionary<int, Delegate>();
        private readonly Dictionary<int, Delegate> _methodCache = new Dictionary<int, Delegate>();
        private readonly IMappingProvider _provider;
        private readonly object locker = new object();

        public Mapper(IMappingProvider provider)
        {
            provider.Register(this);
            _provider = provider;
        }

        internal static Expression GetMapFunctionExpression(IMappingProvider provider, Expression input, Expression output)
        {
            var map = GetMapInternal(input.Type, output.Type);
            return Expression.Call(
                Expression.Constant(provider.CurrentMapper),
                map,
                input,
                output,
                provider.CurrentJob.CacheParameter
            );
        }

        public TOutput Map<TInput, TOutput>(TInput input, TOutput output)
        {
            return MapInternal(input, output, null);
        }

        public TOutput Map<TInput, TOutput>(TInput input)
        {
            return Map(input, default(TOutput));
        }

        public TOutput Map<TOutput>(object input)
        {
            return Map(input, default(TOutput));
        }

        public TOutput Map<TOutput>(object input, TOutput output)
        {
            if (input == null)
                return default;

            var hash = TypePair.CalculateHash(input.GetType(), typeof(TOutput));
            lock (locker)
            {
                if (!_methodCache.ContainsKey(hash))
                {
                    var mapFn = GetMapInternal(input.GetType(), typeof(TOutput));
                    var inputParam = Expression.Parameter(typeof(object));
                    var body = Expression.Lambda(Expression.Call(
                        Expression.Constant(this),
                        mapFn,
                        Expression.Convert(inputParam, input.GetType()),
                        Expression.Default(typeof(TOutput)),
                        Expression.Constant(null, typeof(Dictionary<object, object>))
                    ), inputParam);

                    _methodCache.Add(hash, body.Compile());
                }
            }

            var map = (Func<object, TOutput>)_methodCache[hash];
            return map(input);
        }

        private static MethodInfo GetMapInternal(Type input, Type output) =>
            typeof(Mapper).GetRuntimeMethods().First(p => p.Name == nameof(Mapper.MapInternal)).MakeGenericMethod(input, output);

        private TOutput MapInternal<TInput, TOutput>(TInput input, TOutput output, Dictionary<object, object> refs)
        {
            MapJob map = null;
            var hash = TypePair.CalculateHash(typeof(TInput), typeof(TOutput));
            lock (locker)
            {
                if (!_compiledCache.ContainsKey(hash))
                {
                    map = new MapJob(typeof(TInput), typeof(TOutput));
                    CreateMapFunction(hash, map);
                }
            }

            var fn = (Func<TInput, TOutput, Dictionary<object, object>, TOutput>)_compiledCache[hash];
            return fn(input, output, refs ?? new Dictionary<object, object>());
        }

        private readonly IDictionary<int, MethodCallExpression> _projectionCache = new Dictionary<int, MethodCallExpression>();

        private void CreateMapFunction(int hash, MapJob job)
        {
            if (_compiledCache.ContainsKey(hash))
                return;

            var fn = _provider.CreateMapFunction(job);
            if (fn != null)
                _compiledCache.Add(hash, fn.Compile());
        }

        private void CreateProjectionCall(int hash, MapJob job, Expression queryExpression)
        {
            lock (locker)
            {
                if (_projectionCache.ContainsKey(hash))
                    return;

                var fn = _provider.CreateProjectionFunction(job);

                var body = Expression.Call(
                    typeof(Queryable),
                    nameof(Queryable.Select),
                    new[] { job.Input, job.Output },
                    queryExpression,
                    fn
                );

                _projectionCache[hash] = Expression.Call(body.Method, queryExpression, body.Arguments[1]);
            }
        }

        public IQueryable<TOutput> Project<TOutput>(IQueryable input)
        {
            var hash = TypePair.CalculateHash(input.ElementType, typeof(TOutput));
            if (!_projectionCache.ContainsKey(hash))
                CreateProjectionCall(hash, new MapJob(input.ElementType, typeof(TOutput)), input.Expression);

            var expr = _projectionCache[hash];
            return input.Provider.CreateQuery<TOutput>(expr);
        }
    }
}