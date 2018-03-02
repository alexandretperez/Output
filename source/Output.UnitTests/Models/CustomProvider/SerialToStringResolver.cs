using Output.Resolvers;
using System.Linq.Expressions;

namespace Output.UnitTests.Models.CustomProvider
{
    public class SerialToStringResolver : IResolver
    {
        public Expression Resolve(Expression input, Expression output)
        {
            if (input.Type != typeof(Serial))
                return null;

            return Expression.Property(input, "FormattedValue");
        }
    }
}