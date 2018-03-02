using System.Collections.Generic;

namespace Output.Configurations
{
    public class MappingConfigurationComparer : IEqualityComparer<IMappingConfiguration>
    {
        public static readonly MappingConfigurationComparer Default = new MappingConfigurationComparer();

        public bool Equals(IMappingConfiguration x, IMappingConfiguration y)
        {
            return x == null || y == null ? false : x.Job == y.Job;
        }

        public int GetHashCode(IMappingConfiguration obj)
        {
            return obj.Job.GetHashCode();
        }
    }
}