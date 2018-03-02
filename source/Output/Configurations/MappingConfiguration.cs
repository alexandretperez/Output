using Output.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Output.Configurations
{
    public class MappingConfiguration<TInput, TOutput> : IMappingConfiguration
    {
        public MappingConfiguration()
        {
            Job = new TypePair(typeof(TInput), typeof(TOutput));
        }

        private readonly HashSet<string> _ignored = new HashSet<string>();

        private readonly Dictionary<string, LambdaExpression> _mappings = new Dictionary<string, LambdaExpression>();

        public TypePair Job { get; }

        public MappingConfiguration<TInput, TOutput> Ignore<TProperty>(Expression<Func<TOutput, TProperty>> output)
        {
            _ignored.Add(GetMemberName(output));
            return this;
        }

        public MappingConfiguration<TInput, TOutput> Instance(Expression<Func<TInput, TOutput>> expression)
        {
            _mappings[".ctor"] = expression;
            return this;
        }

        public MappingConfiguration<TInput, TOutput> Map<TProperty>(Expression<Func<TOutput, TProperty>> output, Expression<Func<TInput, TProperty>> input)
        {
            _mappings[GetMemberName(output)] = input;
            return this;
        }

        protected static string GetMemberName<T, TProperty>(Expression<Func<T, TProperty>> property)
        {
            var me = property.Body as MemberExpression ?? (property.Body as UnaryExpression)?.Operand as MemberExpression;
            var name = me?.Member.Name;
            if (name == null)
                throw new ArgumentException("Could not resolve the member", nameof(property));

            return name;
        }

        public bool Resolve(string propertyName, Expression input, out Expression result, IEqualityComparer<string> propertyNameComparer = null)
        {
            result = null;

            var ignored = _ignored;
            var mappings = _mappings;

            if (propertyNameComparer != null)
            {
                ignored = new HashSet<string>(ignored, StringComparer.OrdinalIgnoreCase);
                mappings = new Dictionary<string, LambdaExpression>(mappings, StringComparer.OrdinalIgnoreCase);
            }

            if (ignored.Contains(propertyName))
                return true;

            if (mappings.ContainsKey(propertyName))
            {
                result = mappings[propertyName].ReplaceParameters(input);
                return true;
            }

            return false;
        }
    }
}