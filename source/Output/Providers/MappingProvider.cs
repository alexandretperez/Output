using Output.Extensions;
using Output.Internals;
using Output.Resolvers;
using Output.Visitors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Output.Providers
{
    public class MappingProvider : ProviderBase
    {
        public event EventHandler<Expression> ExpressionReady;

        public override LambdaExpression CreateMapFunction(MapJob job)
        {
            Reset();

            CurrentJob = job;
            DetermineTracking(job.Input);

            LambdaExpression plan;
            if (job.Input.IsEnumerable() && job.Output.IsEnumerable())
            {
                plan = CollectionMap();
            }
            else
            {
                plan = ClassMap();
            }

            ExpressionReady?.Invoke(this, plan);
            CurrentJob = null;

            return plan;
        }

        public override LambdaExpression CreateProjectionFunction(MapJob job)
        {
            Reset();
            var previousJob = CurrentJob;

            job.IsProjecting = true;
            CurrentJob = job;

            DetermineAssignments();

            var bindings = job.GetBindings();

            var ctor = Expression.MemberInit(Expression.New(job.Output), bindings);

            CurrentJob = previousJob;
            return Expression.Lambda(ctor, job.InputParameter);
        }

        protected virtual LambdaExpression ClassMap()
        {
            DetermineAssignments();

            var input = CurrentJob.InputParameter;
            var output = CurrentJob.OutputParameter;
            _ = Expression.Label(CurrentJob.Output, "_return");

            var nullCheck = Expression.NotEqual(input, Expression.Constant(null, input.Type));
            var ctorResolver = GetConstructorResolver();
            var ctor = CurrentJob.GetConstructor(ctorResolver);
            var newInstance = Expression.Assign(output, Expression.Coalesce(output, ctor));
            Expression body;
            if (Tracking.ContainsKey(CurrentJob.Input) && Tracking[CurrentJob.Input])
            {
                body = GetCacheBlock(newInstance, nullCheck);
            }
            else
            {
                body = Expression.Block(
                    Expression.IfThen(nullCheck, Expression.Block(new Expression[] { newInstance }.Concat(CurrentJob.GetAssignments()))),
                    output
                );
            }

            return Expression.Lambda(body, input, output, CurrentJob.CacheParameter);
        }

        protected virtual LambdaExpression CollectionMap()
        {
            var resolver = CurrentJob.Input.IsDictionary()
                ? new DictionaryResolver(this)
                : new CollectionResolver(this) as IResolver;

            var body = resolver.Resolve(CurrentJob.InputParameter, CurrentJob.OutputParameter);
            if (body != null)
                return Expression.Lambda(body, CurrentJob.InputParameter, CurrentJob.OutputParameter, CurrentJob.CacheParameter);

            return null;
        }

        protected virtual Expression GetByFlattening(Expression input, string name)
        {
            var chunks = Utils.SplitPascalCase(name);
            if (chunks.Count == 1)
                return null;

            var member = Flatten(input, chunks);
            if (member == null)
                return null;

            if (CurrentJob.IsProjecting)
                return member;

            return new NullCheckVisitor(member).Visit();
        }

        protected virtual Expression GetMember(Expression input, string name)
        {
            if (input.Type.GetRuntimeProperty(name) != null || input.Type.GetRuntimeField(name) != null)
                return Expression.PropertyOrField(input, name);

            var method = input.Type.GetRuntimeMethod(name, Utils.EmptyTypes());
            return method == null ? null : Expression.Call(input, method);
        }

        protected virtual void ResolveByUnflattening(List<MemberExpression> unresolvedMembers)
        {
            foreach (var prop in unresolvedMembers)
                UnFlatten(prop);
        }

        private void DetermineAssignments()
        {
            var config = Configurations.FirstOrDefault(p => p.Job == CurrentJob);
            var unresolvedMembers = new List<MemberExpression>();
            var nullPropagVisitor = new DefaultValuePropagationVisitor();

            foreach (var prop in CurrentJob.Output.GetRuntimeProperties())
            {
                if (!prop.IsPublic()) continue;

                var name = prop.Name;
                var outputMember = Expression.Property(CurrentJob.OutputParameter, name);

                if (config != null && config.Resolve(name, CurrentJob.InputParameter, out Expression inputResult))
                {
                    if (inputResult == null) // ignored!
                        continue;

                    if (!CurrentJob.IsProjecting)
                        inputResult = nullPropagVisitor.Visit(inputResult);

                    Assign(prop, inputResult, outputMember);
                    continue;
                }

                var inputMember = GetMember(CurrentJob.InputParameter, name) ?? GetByFlattening(CurrentJob.InputParameter, name);

                if (inputMember == null)
                {
                    unresolvedMembers.Add(outputMember);
                    continue;
                }

                Assign(prop, inputMember, outputMember);
            }

            if (unresolvedMembers.Count > 0)
                ResolveByUnflattening(unresolvedMembers);
        }

        private void Assign(PropertyInfo prop, Expression inputMember, MemberExpression outputMember)
        {
            foreach (var resolver in GetResolvers())
            {
                var result = resolver.Resolve(inputMember, outputMember);
                if (result != null)
                {
                    CurrentJob.AssignTo(prop, result);
                    break;
                }
            }
        }

        private Expression Flatten(Expression input, IEnumerable<string> chunks)
        {
            var sb = new StringBuilder();
            var members = new Stack<string>();
            var len = 0;
            foreach (var name in chunks)
            {
                sb.Append(name);
                members.Push(sb.ToString());
                len++;
            }

            Expression result = null;
            foreach (var name in members)
            {
                result = GetMember(input, name);
                if (result != null)
                {
                    chunks = chunks.Skip(len);
                    if (!chunks.Any())
                        break;

                    result = Flatten(result, chunks);
                    if (result != null)
                        break;
                }

                len--;
            }

            return result;
        }

        private Expression GetCacheBlock(BinaryExpression newInstance, BinaryExpression nullCheck)
        {
            var refsOut = Expression.Variable(typeof(object), "reference");
            var refsTryGet = typeof(Dictionary<object, object>).GetRuntimeMethod("TryGetValue", new[] { typeof(object), typeof(object).MakeByRefType() });
            var refsCheck = Expression.Call(CurrentJob.CacheParameter, refsTryGet, CurrentJob.InputParameter, refsOut);
            var refsAdd = Expression.Call(CurrentJob.CacheParameter, CurrentJob.CacheParameter.Type.GetRuntimeMethod("Add", new[] { typeof(object), typeof(object) }), CurrentJob.InputParameter, CurrentJob.OutputParameter);
            var refsBlock = Expression.IfThenElse(refsCheck,
                Expression.Assign(CurrentJob.OutputParameter, Expression.Convert(refsOut, CurrentJob.Output)),
                Expression.Block(new Expression[] { newInstance, refsAdd }.Concat(CurrentJob.GetAssignments()))
            );

            return Expression.Block(
                new[] { refsOut },
                Expression.IfThen(nullCheck, refsBlock),
                CurrentJob.OutputParameter
            );
        }

        private void ProcessUnflattenProperties(MemberExpression member, string memberFullName, MemberInfo[] processedMembers)
        {
            foreach (var prop in member.Type.GetRuntimeProperties())
            {
                if (!prop.IsPublic()) continue;

                if (processedMembers.Contains(prop))
                {
                    if (prop.PropertyType.IsClass() && prop.PropertyType != typeof(string))
                        ProcessUnflattenProperties(GetMember(member, prop.Name) as MemberExpression, memberFullName + prop.Name, processedMembers);

                    continue;
                }

                if (!(GetMember(member, prop.Name) is MemberExpression outputMember))
                    continue;

                var inputMember = GetMember(CurrentJob.InputParameter, memberFullName + prop.Name);
                if (inputMember == null)
                {
                    UnFlatten(outputMember);
                    continue;
                }

                CurrentJob.AssignTo(outputMember, inputMember);
            }
        }

        private void UnFlatten(MemberExpression member)
        {
            if (member.Type.IsConstructedGenericType && typeof(IEnumerable).IsAssignableFrom(member.Type))
                return;

            var processedMembers = new MemberInfo[] { };
            var buildedTypes = new List<Type>();

            var memberFullName = string.Concat(member.ToString().Split('.').Skip(1));
            var constructors = member.Type.GetConstructors();
            if (constructors.Any())
            {
                var ctorResolver = GetConstructorResolver();
                var ctor = ctorResolver.Resolve(CurrentJob.InputParameter, member, memberFullName);
                if (ctor != null)
                {
                    CurrentJob.AssignTo(member, ctor);
                    processedMembers = ctorResolver.GetProcessedMembers();
                    buildedTypes.Add(member.Type);
                }
                else
                {
                    return;
                }
            }

            ProcessUnflattenProperties(member, memberFullName, processedMembers);
        }
    }
}