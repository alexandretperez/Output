using System.Collections.Generic;
using System.Linq.Expressions;

namespace Output.Configurations
{
    public interface IMappingConfiguration
    {
        TypePair Job { get; }

        bool Resolve(string propertyName, Expression input, out Expression result, IEqualityComparer<string> propertyNameComparer = null);
    }
}