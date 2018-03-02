using Output.Extensions;
using System;
using System.Linq.Expressions;

namespace Output.Resolvers
{
    using static Expression;

    public class TypesAssignableResolver : IResolver
    {
        public Expression Resolve(Expression input, Expression output)
        {
            if (input.Type == output.Type)
                return input;

            if (output.Type.IsAssignableFrom(input.Type))
                return Convert(input, output.Type);

            Expression result = null;
            if (input.Type == Nullable.GetUnderlyingType(output.Type))
                result = Convert(input, output.Type);

            var inputUnderType = Nullable.GetUnderlyingType(input.Type);
            if (inputUnderType != null)
            {
                input = Coalesce(input, Default(inputUnderType));
                if (input.Type == output.Type)
                    result = input;
            }

            return result;
        }
    }
}