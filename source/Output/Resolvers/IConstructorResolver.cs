using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Output.Resolvers
{
    public interface IConstructorResolver : IResolver
    {
        Expression Resolve(Expression input, Expression output, string prefix);

        Dictionary<ConstructorInfo, NewExpression> GetProcessedConstructors();

        MemberInfo[] GetProcessedMembers();
    }
}