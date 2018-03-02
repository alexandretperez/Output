using Output.Extensions;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Output.Resolvers
{
    public class PrimitiveResolver : IResolver
    {
        private bool IsTypeValid(Type type)
        {
            return type.IsPrimitive() || type == typeof(decimal);
        }

        public Expression Resolve(Expression input, Expression output)
        {
            if (!IsTypeValid(input.Type) || !IsTypeValid(output.Type))
                return null;

            try
            {
                return Expression.Convert(input, output.Type); // by cast
            }
            catch { };

            var converter = typeof(Convert).GetRuntimeMethod($"To{output.Type.Name}", new[] { input.Type });
            if (converter == null)
                return null;

            return Expression.Call(converter, input);
        }
    }
}