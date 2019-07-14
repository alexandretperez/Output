using Microsoft.Extensions.DependencyInjection;
using Output.Providers;
using System;

namespace Output
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOutputMapper(this IServiceCollection services)
        {
            return AddOutputMapper(services, new MappingProvider());
        }

        public static IServiceCollection AddOutputMapper(this IServiceCollection services, IMappingProvider mappingProvider)
        {
            return services.AddSingleton<IMapper>(new Mapper(mappingProvider));
        }

        public static IServiceCollection AddOutputMapper(this IServiceCollection services, Action<IMappingProvider> options)
        {
            var provider = new MappingProvider();
            options(provider);
            return AddOutputMapper(services, provider);
        }
    }
}
