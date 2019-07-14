using Output.Extensions;
using Output.Internals;
using Output.Providers;
using System;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

namespace Output.Resolvers
{
    public class DictionaryResolver : IResolver
    {
        private readonly IMappingProvider _provider;

        public DictionaryResolver(IMappingProvider provider)
        {
            _provider = provider;
        }

        public Expression Resolve(Expression input, Expression output)
        {
            if (!input.Type.IsDictionary() || !output.Type.IsDictionary())
                return null;

            var outputArgs = output.Type.GetArguments();

            var inputParam = Expression.Parameter(input.Type, "inputParam");
            var getEnumerator = Expression.Call(inputParam, input.Type.GetRuntimeMethod("GetEnumerator", Utils.EmptyTypes()));
            var enumerator = Expression.Variable(getEnumerator.Type, "enumerator");
            var moveNext = Expression.Call(enumerator, typeof(IEnumerator).GetRuntimeMethod("MoveNext", Utils.EmptyTypes()));
            var current = Expression.Property(enumerator, "Current");
            var loopBreak = Expression.Label("_break");

            var outputVar = Expression.Variable(output.Type, "result");
            var outputAdd = Expression.Call(
                outputVar,
                output.Type.GetRuntimeMethod("Add", outputArgs),
                ResolveProperty(Expression.Property(current, "Key"), outputArgs[0]),
                ResolveProperty(Expression.Property(current, "Value"), outputArgs[1])
            );

            return Expression.Block(
                new[] { outputVar, inputParam, enumerator },
                Expression.Assign(inputParam, input),
                Expression.Assign(outputVar, Expression.New(output.Type)),
                Expression.Assign(enumerator, getEnumerator),
                Expression.Loop(
                    Expression.IfThenElse(
                        Expression.IsTrue(moveNext),
                        outputAdd,
                        Expression.Break(loopBreak)
                    ),
                    loopBreak
                ),
                outputVar
            );
        }

        private Expression ResolveProperty(Expression keyOrValue, Type outputType)
        {
            if (outputType.IsAssignableFrom(keyOrValue.Type))
                return keyOrValue;

            if (keyOrValue.Type.IsClass())
                return Mapper.GetMapFunctionExpression(_provider, keyOrValue, Expression.Default(outputType));

            return Expression.Convert(keyOrValue, outputType);
        }
    }
}