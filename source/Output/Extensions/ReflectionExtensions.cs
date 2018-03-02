using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Output.Extensions
{
    internal static class ReflectionExtensions
    {
        public static bool IsPublic(this PropertyInfo prop)
        {
            return prop.CanRead && prop.CanWrite && prop.SetMethod.IsPublic && prop.GetMethod.IsPublic;
        }

        public static bool IsClass(this Type type) => type.GetTypeInfo().IsClass;

        public static IEnumerable<ConstructorInfo> GetConstructors(this Type type) => type.GetTypeInfo().DeclaredConstructors.Where(p => p.IsPublic && !p.IsStatic);

        public static ConstructorInfo GetConstructor(this Type type, params Type[] types) => GetConstructors(type).FirstOrDefault(p => p.MatchParams(types));

        private static bool MatchParams<T>(this T type, Type[] types) where T : MethodBase
        {
            var parameters = type.GetParameters();
            if (parameters.Length != types.Length)
                return false;

            return parameters.Select(p => p.ParameterType).SequenceEqual(types)
                || MatchParamsByAssignableFrom(parameters, types);
        }

        private static bool MatchParamsByAssignableFrom(ParameterInfo[] parameters, Type[] types)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                if (!parameters[i].ParameterType.IsAssignableFrom(types[i]))
                    return false;
            }

            return true;
        }

        public static bool IsNullable(this Type type) => Nullable.GetUnderlyingType(type) != null;

        public static bool IsVisible(this Type type) => type.GetTypeInfo().IsVisible;

        public static bool IsAssignableFrom(this Type type, Type other) => type.GetTypeInfo().IsAssignableFrom(other.GetTypeInfo());

        public static bool IsAssignableFrom(this Type[] types, Type[] others) => types.Length == others.Length && types.Select((type, index) => IsAssignableFrom(type, others[index])).All(p => p);

        public static bool IsEnum(this Type type) => type.GetTypeInfo().IsEnum;

        /// <summary>
        /// Boolean, Byte, SByte, Int16, UInt16, Int32, UInt32, Int64, UInt64, IntPtr, UIntPtr, Char, Double, and Single.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsPrimitive(this Type type) => type.GetTypeInfo().IsPrimitive;

        // int types
        public static bool IsIntegral(this Type type) =>
            IsPrimitive(type)
            && type != typeof(bool)
            && type != typeof(IntPtr)
            && type != typeof(UIntPtr)
            && type != typeof(double)
            && type != typeof(Single);

        public static bool IsQueryable(this Type type) => typeof(IQueryable).IsAssignableFrom(type);

        public static bool IsEnumerable(this Type type) => typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string);

        public static bool IsDictionary(this Type type) => type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition().IsAssignableFrom(typeof(Dictionary<,>));

        public static Type[] GetArguments(this Type type) => type.IsArray ? new[] { type.GetElementType() } : type.GenericTypeArguments;

        public static bool IsCommonType(this Type type)
        {
            if (type == null)
                return false;

            return
                type.IsPrimitive()
                || type == typeof(string)
                || type == typeof(decimal)
                || type == typeof(DateTime)
                || type == typeof(object)
                || type.IsEnum()
                || Nullable.GetUnderlyingType(type) != null
                || IsCommonType(Nullable.GetUnderlyingType(type));
        }
    }
}