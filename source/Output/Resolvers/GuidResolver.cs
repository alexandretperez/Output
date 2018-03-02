using Output.Extensions;
using Output.Internals;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Output.Resolvers
{
    public class GuidResolver : IResolver
    {
        public Expression Resolve(Expression input, Expression output)
        {
            var guid = typeof(Guid);
            if (input.Type != guid && output.Type != guid)
                return null;

            if (output.Type == typeof(string))
                return Expression.Call(input, guid.GetRuntimeMethod("ToString", Utils.EmptyTypes()));

            if (output.Type == typeof(byte[]))
                return Expression.Call(input, guid.GetRuntimeMethod("ToByteArray", Utils.EmptyTypes()));

            if (input.Type == typeof(string) || input.Type == typeof(byte[]))
            {
                return Expression.Condition(
                    Expression.Equal(input, Expression.Constant(null, input.Type)),
                    Expression.Field(null, guid.GetRuntimeField("Empty")),
                    Expression.New(guid.GetConstructor(input.Type), input)
                );
            }

            return null;
        }
    }
}