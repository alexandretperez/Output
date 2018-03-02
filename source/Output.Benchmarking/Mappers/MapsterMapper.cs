using Mapster;
using Output.Benchmarking.Models.Complex;
using Output.Benchmarking.Models.Custom;
using System;
using System.Linq;

namespace Output.Benchmarking.Mappers
{
    public class MapsterMapper<TSource, TTarget> : ITypeMapper<TSource, TTarget> where TTarget : class
    {
        private Adapter adapter;

        public MapsterMapper(string name)
        {
            switch (name)
            {
                case nameof(Tests.ComplexTest):
                case nameof(Tests.IntenseTest):
                    ConfigMovie();
                    break;

                case nameof(Tests.CustomTest):
                    ConfigEmployee();
                    break;

                default:
                    adapter = new Adapter(new TypeAdapterConfig());
                    break;
            }
        }

        private void ConfigEmployee()
        {
            var config = new TypeAdapterConfig();
            config.NewConfig<Employee, EmployeeDto>()
             .ConstructUsing(p => new EmployeeDto(true))
             .Map(p => p.FullName, p => $"{p.Name} {p.Surname}")
             .Map(p => p.IsSociable, p => p.Skills.Contains("Sociable"))
             .Map(p => p.Contact, p => $"Address: {p.Address}, Phone: {p.Phone}")
             .Map(p => p.Age, p => (int)Math.Floor((DateTime.Today - p.BirthDate).TotalDays / 365.2425))
             .Ignore(p => p.Email)
             .Compile();

            adapter = new Adapter(config);
        }

        private void ConfigMovie()
        {
            var config = new TypeAdapterConfig();
            config.NewConfig<Movie, MovieDto>()
                .PreserveReference(true)
                .Compile();

            adapter = new Adapter(config);
        }

        public TTarget Map(TSource source)
        {
            return adapter.Adapt<TSource, TTarget>(source);
        }
    }
}