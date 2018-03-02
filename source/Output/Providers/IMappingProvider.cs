using Output.Configurations;
using Output.Resolvers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Output.Providers
{
    public interface IMappingProvider
    {
        LambdaExpression CreateMapFunction(MapJob job);

        LambdaExpression CreateProjectionFunction(MapJob job);

        void Register(IMapper mapper);

        MapJob CurrentJob { get; }

        IMapper CurrentMapper { get; }

        IEnumerable<IResolver> GetResolvers();

        IMappingProvider AddConfig(IMappingConfiguration config);

        IMappingProvider AddConfig<TInput, TOutput>(Action<MappingConfiguration<TInput, TOutput>> action);

        bool RemoveConfig(IMappingConfiguration config);

        bool RemoveConfig<TInput, TOutput>();
    }
}