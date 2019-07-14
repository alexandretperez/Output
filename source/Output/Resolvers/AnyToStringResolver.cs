using Output.Extensions;
using Output.Visitors;
using System.Linq.Expressions;
using System.Reflection;

namespace Output.Resolvers
{
    public class AnyToStringResolver : IResolver
    {
        public Expression Resolve(Expression input, Expression output)
        {
            if (input.Type == typeof(string) || output.Type != typeof(string))
                return null;

            Expression result = Expression.Call(input, typeof(object).GetTypeInfo().GetDeclaredMethod("ToString"));
            if (input.Type.IsClass())
                result = new NullCheckVisitor(result).Visit();

            return result;
        }
    }
}