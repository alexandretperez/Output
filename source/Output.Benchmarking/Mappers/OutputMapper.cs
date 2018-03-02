using Output.Benchmarking.Models.Custom;
using Output.Configurations;
using Output.Providers;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Output.Benchmarking.Mappers
{
    public class OutputMapper<TSource, TTarget> : ITypeMapper<TSource, TTarget> where TTarget : class
    {
        private readonly IMapper _mapper;

        public OutputMapper(string name)
        {
            var provider = new MappingProvider();
#if DEBUG
            provider.ExpressionReady += (sender, expr) =>
            {
                var debugViewProp = typeof(Expression).GetProperty("DebugView", BindingFlags.Instance | BindingFlags.NonPublic);
                var debugViewGetter = debugViewProp.GetGetMethod(true);
                Console.WriteLine(debugViewGetter.Invoke(expr, null));
            };
#endif
            if (name == nameof(Tests.CustomTest))
                ConfigEmployee(provider);

            _mapper = new Mapper(provider);
        }

        private void ConfigEmployee(MappingProvider provider)
        {
            var config = new MappingConfiguration<Employee, EmployeeDto>()
                .Instance(p => new EmployeeDto(true))
                .Map(p => p.FullName, p => $"{p.Name} {p.Surname}")
                .Map(p => p.IsSociable, p => p.Skills.Contains("Sociable"))
                .Map(p => p.Contact, p => $"Address: {p.Address}, Phone: {p.Phone}")
                .Map(p => p.Age, p => (int)Math.Floor((DateTime.Today - p.BirthDate).TotalDays / 365.2425))
                .Ignore(p => p.Email);

            provider.AddConfig(config);
        }

        public TTarget Map(TSource source)
        {
            return _mapper.Map<TSource, TTarget>(source);
        }
    }
}