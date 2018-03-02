using System.Linq.Expressions;

namespace Output.Resolvers
{
    public interface IResolver
    {
        Expression Resolve(Expression input, Expression output);
    }
}