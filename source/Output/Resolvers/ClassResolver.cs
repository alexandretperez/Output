using Output.Extensions;
using Output.Providers;
using System.Linq.Expressions;
using System.Reflection;

namespace Output.Resolvers
{
    public class ClassResolver : IResolver
    {
        private readonly IMappingProvider _provider;

        public ClassResolver(IMappingProvider provider)
        {
            _provider = provider;
        }

        public Expression Resolve(Expression input, Expression output)
        {
            if (!input.Type.IsClass() || !output.Type.IsClass() || input.Type.IsCommonType() || output.Type.IsCommonType())
                return null;

            if (_provider.CurrentJob.IsProjecting)
                return Project(input, output);

            return Mapper.GetMapFunctionExpression(_provider, input, output);
        }

        private Expression Project(Expression input, Expression output)
        {
            var fn = _provider.CreateProjectionFunction(new MapJob(input.Type, output.Type));
            return fn.ReplaceParameters(input);
        }
    }
}