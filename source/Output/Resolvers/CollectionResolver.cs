using Output.Extensions;
using Output.Internals;
using Output.Providers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Output.Resolvers
{
    public class CollectionResolver : IResolver
    {
        private Type _inputArg;
        private Type _outputArg;
        private bool _skipCoalesce;
        private readonly IMappingProvider _provider;

        public CollectionResolver(IMappingProvider provider)
        {
            _provider = provider;
        }

        public Expression Resolve(Expression input, Expression output)
        {
            if (!input.Type.IsEnumerable() || !output.Type.IsEnumerable() || input.Type.IsDictionary() || output.Type.IsDictionary())
                return null;

            _inputArg = input.Type.GetArguments()[0];
            _outputArg = output.Type.GetArguments()[0];

            if (_provider.CurrentJob.IsProjecting)
                return Project(input, output);

            if (_inputArg.IsAssignableFrom(_outputArg))
                return Convert(input, output.Type);

            return Iterate(input, output);
        }

        private Expression Project(Expression input, Expression output)
        {
            var job = new MapJob(_inputArg, _outputArg);

            var fn = _provider.CreateProjectionFunction(job);
            var type = input.Type.IsQueryable() ? typeof(Queryable) : typeof(Enumerable);
            var expr = Expression.Call(type, "Select", new[] { fn.Parameters[0].Type, fn.ReturnType }, input, fn);

            if (expr.Type == output.Type)
                return expr;

            if (output.Type.IsArray)
                return Expression.Call(type, "ToArray", new[] { _outputArg }, expr);

            return Expression.Call(type, "ToList", new[] { _outputArg }, expr);
        }

        private Expression Convert(Expression input, Type result)
        {
            var parameter = _skipCoalesce ? input : Expression.Coalesce(input, input.Init());
            if (result.IsArray)
                return Expression.Call(typeof(Enumerable), "ToArray", new[] { _outputArg }, parameter);

            if (result.IsConstructedGenericType)
            {
                if (typeof(IQueryable<>).MakeGenericType(_outputArg).IsAssignableFrom(result))
                    return Expression.Call(typeof(Queryable), "AsQueryable", new[] { _outputArg }, parameter);

                if (typeof(ICollection<>).MakeGenericType(_outputArg).IsAssignableFrom(result)
                    || typeof(IReadOnlyCollection<>).MakeGenericType(_outputArg).IsAssignableFrom(result))
                {
                    var ctor = result.GetConstructor(result);
                    if (ctor != null)
                        return Expression.New(ctor, parameter);

                    var list = Expression.New(typeof(List<>).MakeGenericType(_outputArg).GetConstructor(result), parameter);
                    return Expression.Convert(list, result);
                }

                if (typeof(IEnumerable<>).MakeGenericType(_outputArg).IsAssignableFrom(result))
                    return Expression.Call(typeof(Enumerable), "AsEnumerable", new[] { _outputArg }, parameter);
            }

            return input;
        }

        public Expression Iterate(Expression input, Expression output)
        {
            if (output.Type.IsArray)
                return IterateOnArray(input, output);

            var safeVar = Expression.Coalesce(input, input.Init());
            var typeGetEnumerator = Expression.Convert(safeVar, typeof(IEnumerable<>).MakeGenericType(_inputArg));
            var getEnumerator = Expression.Call(typeGetEnumerator, typeGetEnumerator.Type.GetRuntimeMethod("GetEnumerator", Utils.EmptyTypes()));
            var enumerator = Expression.Variable(getEnumerator.Type, "enumerator");
            var moveNext = Expression.Call(enumerator, typeof(IEnumerator).GetRuntimeMethod("MoveNext", Utils.EmptyTypes()));
            var loopBreak = Expression.Label("_break");
            var current = Expression.Convert(Expression.Property(enumerator, "Current"), _inputArg);

            var result = Expression.Variable(typeof(List<>).MakeGenericType(_outputArg), "result");
            var item = Expression.Default(_outputArg);

            var mapFn = Mapper.GetMapFunctionExpression(_provider, current, item);

            var loop = Expression.Loop(
                Expression.IfThenElse(
                    Expression.IsTrue(moveNext),
                    Expression.Call(result, result.Type.GetRuntimeMethod("Add", new[] { _outputArg }), mapFn),
                    Expression.Break(loopBreak)
                ),
                loopBreak
            );

            var newExpr = Expression.New(result.Type);

            _skipCoalesce = true;
            return Expression.Block(
                new[] { result, enumerator },
                Expression.Assign(enumerator, getEnumerator),
                Expression.Assign(result, newExpr),
                loop,
                result.Type == output.Type
                    ? result
                    : Convert(result, output.Type)
            );
        }

        private Expression IterateOnArray(Expression input, Expression output)
        {
            var count = typeof(Enumerable).GetRuntimeMethods().First(p => p.Name == "Count" && p.IsStatic && p.GetParameters().Length == 1).MakeGenericMethod(_inputArg);

            var data = Expression.Variable(input.Type, "data");
            var init = Expression.NewArrayBounds(_outputArg, Expression.Call(null, count, data));

            var enumGet = Expression.Call(data, input.Type.GetRuntimeMethod("GetEnumerator", Utils.EmptyTypes()));
            var enumVar = Expression.Variable(enumGet.Type, "enumerator");
            var enumMoveNext = Expression.Call(enumVar, typeof(IEnumerator).GetRuntimeMethod("MoveNext", Utils.EmptyTypes()));
            var enumCurrent = Expression.Convert(Expression.Property(enumVar, "Current"), _inputArg);
            var loopBreak = Expression.Label("_break");

            var array = Expression.Variable(output.Type, "result");
            var index = Expression.Variable(typeof(int), "index");
            var mapFn = Mapper.GetMapFunctionExpression(_provider, enumCurrent, Expression.Default(_outputArg));

            return Expression.Block(
                new[] { data, array, enumVar, index },
                    Expression.Assign(data, Expression.Coalesce(input, input.Init())),
                    Expression.Assign(array, init),
                    Expression.Assign(enumVar, enumGet),
                    Expression.Loop(
                        Expression.IfThenElse(
                                Expression.IsTrue(enumMoveNext),
                                Expression.Assign(
                                    Expression.ArrayAccess(array, Expression.PostIncrementAssign(index)),
                                    mapFn
                                ),
                                Expression.Break(loopBreak)
                        ),
                        loopBreak
                    ),
                    array
            );
        }
    }
}