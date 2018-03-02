using Output.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Output.Internals
{
    internal static class Utils
    {
        public static Type[] EmptyTypes() => new Type[] { };

        public static List<string> SplitPascalCase(string current)
        {
            if (string.IsNullOrEmpty(current))
                return new List<string>();

            var result = new List<string>();
            var k = 0;
            for (int i = 1, j = current.Length; i < j; i++)
            {
                if (char.IsUpper(current[i]))
                {
                    result.Add(current.Substring(k, i - k));
                    k = i;
                }
            }

            result.Add(current.Substring(k));
            return result;
        }

        public static Type[] ExtractTypes(Type type)
        {
            if (typeof(IEnumerable).IsAssignableFrom(type) && type.IsConstructedGenericType)
                return type.GetArguments();

            return new[] { type };
        }
    }
}