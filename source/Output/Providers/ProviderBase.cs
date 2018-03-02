using Output.Configurations;
using Output.Extensions;
using Output.Internals;
using Output.Resolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Output.Providers
{
    public abstract class ProviderBase : IMappingProvider
    {
        protected readonly HashSet<IMappingConfiguration> Configurations = new HashSet<IMappingConfiguration>(MappingConfigurationComparer.Default);

        protected readonly Dictionary<Type, bool> Tracking = new Dictionary<Type, bool>();

        private IConstructorResolver _ctorResolver;

        public MapJob CurrentJob { get; protected set; }

        public IMapper CurrentMapper { get; private set; }

        public abstract LambdaExpression CreateMapFunction(MapJob job);

        public abstract LambdaExpression CreateProjectionFunction(MapJob job);

        public virtual IEnumerable<IResolver> GetResolvers()
        {
            yield return new TypesAssignableResolver();
            yield return new PrimitiveResolver();
            yield return new GuidResolver();
            yield return new EnumResolver();
            yield return new DictionaryResolver(this);
            yield return new CollectionResolver(this);
            yield return new ClassResolver(this);
            yield return new AnyToStringResolver();
        }

        public void Register(IMapper mapper)
        {
            CurrentMapper = mapper;
        }

        protected void DetermineTracking(Type input)
        {
            if (!Tracking.ContainsKey(input))
                Tracking.Add(input, false);

            foreach (var prop in input.GetRuntimeProperties().Where(p => p.PropertyType.IsClass() && !p.PropertyType.IsCommonType()).ToList())
            {
                foreach (var p in Utils.ExtractTypes(prop.PropertyType))
                {
                    if (Tracking.ContainsKey(p))
                    {
                        Tracking[p] = true;
                        continue;
                    }

                    DetermineTracking(p);
                }
            }
        }

        protected virtual IConstructorResolver GetConstructorResolver()
        {
            return _ctorResolver ?? (_ctorResolver = new ConstructorResolver(this, Configurations.FirstOrDefault(p => p.Job == CurrentJob)));
        }

        protected void Reset()
        {
            _ctorResolver = null;
            Tracking.Clear();
        }

        
        public IMappingProvider AddConfig(IMappingConfiguration config)
        {
            EnsureNoMapper();
            if (Configurations.Contains(config))
                throw new Exception($"A configuration between [{config.Job.Input.FullName}] and [{config.Job.Output.FullName}] has already been added.");

            Configurations.Add(config);
            return this;
        }

        public IMappingProvider AddConfig<TInput, TOutput>(Action<MappingConfiguration<TInput, TOutput>> action)
        {
            var config = new MappingConfiguration<TInput, TOutput>();
            action(config);
            AddConfig(config);
            return this;
        }

        public bool RemoveConfig(IMappingConfiguration config)
        {
            EnsureNoMapper();
            return Configurations.Remove(config);
        }

        public bool RemoveConfig<TInput, TOutput>()
        {
            EnsureNoMapper();
            return Configurations.RemoveWhere(config => config.Job.Input == typeof(TInput) && config.Job.Output == typeof(TOutput)) > 0;
        }

        private void EnsureNoMapper()
        {
            if (CurrentMapper != null)
                throw new InvalidOperationException("You cannot change provider configurations after the Mapper instantiation.");
        }
    }
}