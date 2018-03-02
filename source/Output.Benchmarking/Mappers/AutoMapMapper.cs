using Output.Benchmarking.Models.Basic;
using Output.Benchmarking.Models.Complex;
using Output.Benchmarking.Models.Custom;
using Output.Benchmarking.Models.Flattening;
using Output.Benchmarking.Models.Intense;
using Output.Benchmarking.Models.Shared;
using System;
using System.Linq;

namespace Output.Benchmarking.Mappers
{
    public class AutoMapMapper<TSource, TTarget> : ITypeMapper<TSource, TTarget> where TTarget : class
    {
        private AutoMapper.IMapper _mapper;

        public AutoMapMapper(string name)
        {
            switch (name)
            {
                case nameof(Tests.BasicTest):
                    ConfigCustomer();
                    break;

                case nameof(Tests.ComplexTest):
                    ConfigMovie();
                    break;

                case nameof(Tests.IntenseTest):
                    ConfigProfile();
                    break;

                case nameof(Tests.CustomTest):
                    ConfigEmployee();
                    break;

                case nameof(Tests.FlatteningTest):
                    ConfigEarth();
                    break;
            }
        }

        private void ConfigEarth()
        {
            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Earth, EarthDto>();
            });

            config.AssertConfigurationIsValid();
            config.CompileMappings();

            _mapper = config.CreateMapper();
        }

        private void ConfigEmployee()
        {
            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Employee, EmployeeDto>()
                    .ForMember(p => p.FullName, p => p.MapFrom(q => q.Name + " " + q.Surname))
                    .ForMember(p => p.IsSociable, p => p.MapFrom(q => q.Skills.Contains("Sociable")))
                    .ForMember(p => p.Email, p => p.Ignore())
                    .ForMember(p => p.Contact, p => p.MapFrom(q => $"Address: {q.Address}, Phone: {q.Phone}"))
                    .ForMember(p => p.Age, p => p.MapFrom(q => (int)Math.Floor((DateTime.Today - q.BirthDate).TotalDays / 365.2425)))
                    .ConstructUsing(c => new EmployeeDto(true));
            });
            config.AssertConfigurationIsValid();
            config.CompileMappings();
            _mapper = config.CreateMapper();
        }

        private void ConfigProfile()
        {
            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Profile, ProfileDto>();
                cfg.CreateMap<Address, AddressDto>();
                cfg.CreateMap<Age, AgeDto>();
                cfg.CreateMap<Email, EmailDto>();
                cfg.CreateMap<Goods, GoodsDto>();
                cfg.CreateMap<House, HouseDto>();
                cfg.CreateMap<Name, NameDto>();
                cfg.CreateMap<Picture, PictureDto>();
                cfg.CreateMap<Room, RoomDto>();
                cfg.CreateMap<Tag, TagDto>();
                cfg.CreateMap<Vehicle, VehicleDto>();

                // config movie
                cfg.CreateMap<Actor, ActorDto>();
                cfg.CreateMap<Director, DirectorDto>();
                cfg.CreateMap<Movie, MovieDto>();
                cfg.CreateMap<MovieGenre, MovieGenreDto>();
            });
            config.AssertConfigurationIsValid();
            config.CompileMappings();
            _mapper = config.CreateMapper();
        }

        private void ConfigMovie()
        {
            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Actor, ActorDto>();
                cfg.CreateMap<Director, DirectorDto>();
                cfg.CreateMap<Movie, MovieDto>();
                cfg.CreateMap<MovieGenre, MovieGenreDto>();
            });
            config.AssertConfigurationIsValid();
            config.CompileMappings();
            _mapper = config.CreateMapper();
        }

        private void ConfigCustomer()
        {
            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Customer, CustomerDto>();
                cfg.CreateMap<Address, AddressDto>();
                cfg.CreateMap<Phone, PhoneDto>();
            });
            config.AssertConfigurationIsValid();
            config.CompileMappings();
            _mapper = config.CreateMapper();
        }

        public TTarget Map(TSource source)
        {
            return _mapper.Map<TSource, TTarget>(source);
        }
    }
}