using Output.Configurations;
using Output.Extensions;
using Output.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Output.Resolvers
{
    public class ConstructorResolver : IConstructorResolver
    {
        private readonly IMappingConfiguration _config;
        private readonly HashSet<MemberInfo> _processedMembers = new HashSet<MemberInfo>();
        private readonly IMappingProvider _provider;
        private bool _constructorChecked;
        private Expression _input;
        private Expression _output;
        private readonly Dictionary<ConstructorInfo, NewExpression> _processedConstructors = new Dictionary<ConstructorInfo, NewExpression>();

        public ConstructorResolver(IMappingProvider provider, IMappingConfiguration config)
        {
            _provider = provider;
            _config = config;
        }

        public MemberInfo[] GetProcessedMembers()
        {
            return _processedMembers.ToArray();
        }

        public Dictionary<ConstructorInfo, NewExpression> GetProcessedConstructors() => _processedConstructors;

        public Expression Resolve(Expression input, Expression output) => Resolve(input, output, "");

        public Expression Resolve(Expression input, Expression output, string prefix)
        {
            if (!_constructorChecked && _config != null && _config.Resolve(".ctor", input, out Expression ctor))
                return ctor;

            _constructorChecked = true;

            var constructor = output.Type.GetConstructor(); // default constructor

            if (constructor != null && output.Type.IsClass())
                return Expression.New(constructor);

            var inputProperties = input.Type.GetRuntimeProperties()
                .Where(p => p.GetMethod.IsPublic && !p.GetMethod.IsStatic && p.Name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                .ToList();

            var outputConstructors = output.Type.GetConstructors()
                .OrderByDescending(p => p.GetParameters().Length)
                .ToArray();

            if (outputConstructors.Length == 0)
                return null;

            _input = input;
            _output = output;

            return FindConstructor(outputConstructors, inputProperties, prefix);
        }

        private Expression FindConstructor(ConstructorInfo[] constructors, List<PropertyInfo> inputProperties, string prefix)
        {
            for (int i = 0; i < constructors.Length; i++)
            {
                var ctor = constructors[i];
                if (_processedConstructors.ContainsKey(ctor))
                    return _processedConstructors[ctor];

                var parameters = ctor.GetParameters();
                var arguments = new List<Expression>();

                foreach (var parameter in parameters)
                {
                    var name = prefix + parameter.Name;

                    if (parameter.ParameterType.IsPointer)
                        continue;

                    var resolution = _provider.CurrentJob.GetResolutions().FirstOrDefault(p => p.Key.Member.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
                    if (resolution.Key != null)
                    {
                        RegisterProcessedMember(parameter.Name, prefix);
                        arguments.Add(resolution.Value);
                        continue;
                    }

                    var prop = inputProperties.Find(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

                    if (_config != null && _config.Resolve(name, _input, out Expression result, StringComparer.OrdinalIgnoreCase))
                    {
                        RegisterProcessedMember(parameter.Name, prefix);
                        arguments.Add(result);
                        continue;
                    }

                    if (prop == null)
                    {
                        result = Resolve(_input, Expression.Default(parameter.ParameterType), prefix + parameter.Name);
                        if (result != null)
                        {
                            RegisterProcessedMember(parameter.Name, prefix);
                            arguments.Add(result);
                        }

                        continue;
                    }

                    foreach (var resolver in _provider.GetResolvers())
                    {
                        result = resolver.Resolve(Expression.Property(_input, prop.Name), Expression.Default(parameter.ParameterType));
                        if (result != null)
                        {
                            RegisterProcessedMember(parameter.Name, prefix);
                            arguments.Add(result);
                            break;
                        }
                    }
                }

                if (arguments.Count == parameters.Length)
                {
                    var ctorExpr = Expression.New(ctor, arguments);
                    _processedConstructors.Add(ctor, ctorExpr);
                    return ctorExpr;
                }
            }

            return null;
        }

        private void RegisterProcessedMember(string name, string prefix)
        {
            var props = _output.Type.GetRuntimeProperties()
                .Where(p =>
                    p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
                    || (prefix + p.Name).Equals(name, StringComparison.OrdinalIgnoreCase)
                );

            foreach (var prop in props)
                _processedMembers.Add(prop);
        }
    }
}