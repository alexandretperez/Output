using Output.Providers;
using Output.Resolvers;
using System.Collections.Generic;

namespace Output.UnitTests.Models.CustomProvider
{
    public class MyCustomMappingProvider : MappingProvider
    {
        public override IEnumerable<IResolver> GetResolvers()
        {
            yield return new SerialToStringResolver();

            foreach (var resolver in base.GetResolvers())
                yield return resolver;
        }
    }
}