using Output.Internals;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Output.Extensions
{
    internal static class ExpressionExtensions
    {
        public static Expression ReplaceParameters(this LambdaExpression lambda, params Expression[] replacer)
        {
            var original = lambda.Parameters;
            var len = Math.Min(original.Count, replacer.Length);
            var result = lambda.Body;
            for (int i = 0; i < len; i++)
                result = new ExpressionReplacerVisitor(original[i], replacer[i]).Visit(result);

            return result;
        }

        public static Expression Init(this Expression expr)
        {
            if (expr.Type.IsArray)
                return Expression.NewArrayInit(expr.Type.GetElementType());

            if (expr.Type.GetConstructor(expr.Type) != null)
                return Expression.New(expr.Type);

            if (expr.Type.IsEnumerable())
            {
                var type = expr.Type.GetArguments()[0];
                return Expression.Convert(Expression.NewArrayInit(type), typeof(IEnumerable<>).MakeGenericType(type));
            }

            return Expression.Default(expr.Type);
        }
    }
}