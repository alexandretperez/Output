using Output.Internals;
using Output.Resolvers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace Output
{
    [DebuggerDisplay("{Input} -> {Output}")]
    public class MapJob : TypePair
    {
        private readonly Dictionary<MemberExpression, Expression> _resolution = new Dictionary<MemberExpression, Expression>();

        public MapJob(Type input, Type output) : base(input, output)
        {
            InputParameter = Expression.Parameter(input, "input");
            OutputParameter = Expression.Parameter(output, "output");
            CacheParameter = Expression.Parameter(typeof(Dictionary<object, object>), "cache");
        }

        public Expression GetConstructor(IConstructorResolver constructorResolver)
        {
            var ctor = constructorResolver.Resolve(InputParameter, OutputParameter);
            if (ctor == null)
                throw new InvalidOperationException($"It was not possible determine a constructor to the type [{Output.FullName}].");

            RemoveProcessedMembers(constructorResolver);
            return ctor;
        }

        private void RemoveProcessedMembers(IConstructorResolver resolver)
        {
            foreach (var ctor in resolver.GetProcessedConstructors())
            {
                foreach (var parameter in ctor.Key.GetParameters())
                {
                    MemberExpression key = null;
                    foreach (var r in _resolution.Keys)
                    {
                        if (r.Member.Name.Equals(parameter.Name, StringComparison.OrdinalIgnoreCase)
                            && r.Type == parameter.ParameterType
                            && ctor.Key.DeclaringType == r.Member.DeclaringType)
                        {
                            key = r;
                            break;
                        }
                    }

                    if (key != null)
                        _resolution.Remove(key);
                }
            }
        }

        public ParameterExpression InputParameter { get; }
        public ParameterExpression OutputParameter { get; }
        public ParameterExpression CacheParameter { get; }

        public void AssignTo(MemberInfo output, Expression input)
        {
            _resolution.Add(Expression.Property(OutputParameter, output.Name), input);
        }

        public void AssignTo(MemberExpression memberExpression, Expression input)
        {
            _resolution.Add(memberExpression, input);
        }

        public IEnumerable<BinaryExpression> GetAssignments()
        {
            foreach (var item in _resolution)
                yield return Expression.Assign(item.Key, item.Value);
        }

        internal Dictionary<MemberExpression, Expression> GetResolutions() => _resolution;

        public IEnumerable<MemberBinding> GetBindings(Expression parameter = null)
        {
            foreach (var item in _resolution)
            {
                yield return Expression.Bind(item.Key.Member,
                    parameter == null
                        ? item.Value
                        : new ParameterReplacerVisitor(parameter).Visit(item.Value));
            }
        }

        public bool IsProjecting { get; set; }
    }
}