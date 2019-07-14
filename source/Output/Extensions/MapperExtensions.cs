using System.Collections.Generic;

namespace Output.Extensions
{
    public static class MapperExtensions
    {
        public static TOutput MapChain<TOutput>(this IMapper mapper, TOutput output, params dynamic[] inputs)
        {
            if (EqualityComparer<TOutput>.Default.Equals(output, default))
                return default;

            foreach (var input in inputs)
                output = mapper.Map(input, output);

            return output;
        }
    }
}