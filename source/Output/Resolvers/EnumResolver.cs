using Output.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace Output.Resolvers
{
    public class EnumResolver : IResolver
    {
        public Expression Resolve(Expression input, Expression output)
        {
            var inputIsEnum = input.Type.IsEnum();
            var outputIsEnum = output.Type.IsEnum();

            if (!inputIsEnum && !outputIsEnum)
                return null;

            if (inputIsEnum && outputIsEnum)
                return Expression.Convert(input, output.Type);

            if (inputIsEnum)
            {
                if (output.Type == typeof(string))
                    return GetString(input);

                if (output.Type.IsIntegral() || output.Type == typeof(object))
                    return Expression.Convert(input, output.Type);
            }

            if (outputIsEnum)
            {
                if (input.Type == typeof(string))
                    return Parse(input, output.Type);

                if (input.Type.IsIntegral() || input.Type == typeof(object))
                    return Expression.Convert(input, output.Type);
            }

            return null;
        }

        private static Expression Parse(Expression value, Type outputType)
        {
            var parseMethod = typeof(Enum).GetRuntimeMethod("Parse", new[] { typeof(Type), typeof(string) });
            var methodCall = Expression.Call(parseMethod, Expression.Constant(outputType), value);
            return Expression.Convert(methodCall, outputType);
        }

        private static Expression GetString(Expression expr)
        {
            var keyType = Enum.GetUnderlyingType(expr.Type);
            var dic = Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(keyType, typeof(string)));
            var converter = typeof(Convert).GetRuntimeMethod("To" + keyType.Name, new[] { typeof(object), typeof(IFormatProvider) });
            foreach (var item in Enum.GetValues(expr.Type))
            {
                dic.GetType().GetRuntimeMethod("Add", new Type[] { keyType, typeof(string) }).Invoke(dic, new[] {
                    converter.Invoke(null, new[] { item, CultureInfo.InvariantCulture }),
                    item.ToString()
                });
            }

            return Expression.MakeIndex(
                Expression.Constant(dic),
                dic.GetType().GetRuntimeProperty("Item"),
                new[] { Expression.Convert(expr, keyType) }
            );
        }
    }
}