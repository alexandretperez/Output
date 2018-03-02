using Output.Benchmarking.Models.Basic;
using Output.Benchmarking.Models.Complex;
using Output.Benchmarking.Models.Custom;
using Output.Benchmarking.Models.Flattening;
using Output.Benchmarking.Models.Intense;
using Output.Benchmarking.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Output.Benchmarking.Mappers
{
    public class HandwrittenMapper<TSource, TTarget> : ITypeMapper<TSource, TTarget> where TTarget : class
    {
        private readonly Func<TSource, TTarget> _fn;

        public HandwrittenMapper(string name)
        {
            switch (name)
            {
                case nameof(Tests.BasicTest):
                    _fn = MapCustomer() as Func<TSource, TTarget>;
                    break;

                case nameof(Tests.ComplexTest):
                    _fn = MapMovie() as Func<TSource, TTarget>;
                    break;

                case nameof(Tests.IntenseTest):
                    _fn = MapProfile() as Func<TSource, TTarget>;
                    break;

                case nameof(Tests.CustomTest):
                    _fn = MapEmployee() as Func<TSource, TTarget>;
                    break;

                case nameof(Tests.FlatteningTest):
                    _fn = MapEarth() as Func<TSource, TTarget>;
                    break;

                default:
                    if (typeof(IEnumerable<Customer>).IsAssignableFrom(typeof(TSource)))
                        _fn = MapCustomerCollection() as Func<TSource, TTarget>;
                    break;
            }
        }

        private static Func<Earth, EarthDto> MapEarth()
        {
            return (Earth earth) =>
            {
                return new EarthDto
                {
                    ContinentCountryName = earth?.Continent?.Country?.Name,
                    ContinentCountryStateName = earth?.Continent?.Country?.State?.Name,
                    ContinentCountryStateCityName = earth?.Continent?.Country?.State?.City?.Name
                };
            };
        }

        private static Func<IEnumerable<Customer>, CustomerDto[]> MapCustomerCollection()
        {
            return (IEnumerable<Customer> customers) => customers.Select(HandwrittenMapperBuilder.GetCustomerDto).ToArray();
        }

        private static Func<Employee, EmployeeDto> MapEmployee()
        {
            return HandwrittenMapperBuilder.GetEmployeeDto;
        }

        private static Func<Profile, ProfileDto> MapProfile()
        {
            return HandwrittenMapperBuilder.GetProfileDto;
        }

        private static Func<Customer, CustomerDto> MapCustomer()
        {
            return HandwrittenMapperBuilder.GetCustomerDto;
        }

        private static Func<Movie, MovieDto> MapMovie()
        {
            return HandwrittenMapperBuilder.GetMovieDto;
        }

        public TTarget Map(TSource source)
        {
            return _fn(source);
        }
    }

    public static class HandwrittenMapperBuilder
    {
        public static MovieDto GetMovieDto(Movie movie)
        {
            var dto = new MovieDto
            {
                Id = movie.Id,
                Name = movie.Name,
                Year = movie.Year,
                Director = new DirectorDto
                {
                    Name = movie.Director.Name,
                    Movies = new List<MovieDto>()
                }
            };

            dto.Director.Movies.Add(dto);

            dto.Actors = movie.Actors.Select(p => new ActorDto
            {
                Name = p.Name
            }).ToArray();

            dto.Genres = movie.Genres.ConvertAll(p => new MovieGenreDto
            {
                Name = p.Name,
                Movies = new List<MovieDto> { dto }
            });

            dto.SoundTracks = movie.SoundTracks.Select(p => new SoundTrackDto
            {
                Name = p.Name,
                Length = p.Length,
                ComposerName = p.Composer.Name
            }).ToArray();

            return dto;
        }

        public static CustomerDto GetCustomerDto(Customer customer)
        {
            var dto = new CustomerDto
            {
                Id = customer.Id,
                HomeAddress = new AddressDto
                {
                    Id = customer.HomeAddress.Id,
                    City = customer.HomeAddress.City,
                    Country = customer.HomeAddress.Country,
                    Street = customer.HomeAddress.Street
                },
                IsActive = customer.IsActive,
                Name = customer.Name,
                PhonesCount = customer.Phones.Count,
                WorkAddressesCount = customer.WorkAddresses.Count
            };

            dto.WorkAddresses = new List<AddressDto>();
            foreach (var address in customer.WorkAddresses)
            {
                dto.WorkAddresses.Add(new AddressDto
                {
                    Id = address.Id,
                    City = address.City,
                    Country = address.Country,
                    Street = address.Street
                });
            }

            dto.Phones = new List<PhoneDto>();
            foreach (var phone in customer.Phones)
            {
                dto.Phones.Add(new PhoneDto
                {
                    Number = phone.Number,
                    Type = phone.Type.ToString()
                });
            }

            return dto;
        }

        public static ProfileDto GetProfileDto(Profile profile)
        {
            var dto = new ProfileDto
            {
                About = profile.About,
                Age = new AgeDto
                {
                    BirthDate = profile.Age.BirthDate,
                    Value = profile.Age.Value
                },
                Balance = profile.Balance,
                Company = profile.Company,
                Email = new EmailDto
                {
                    Value = profile.Email.Value,
                    IsEmailValid = profile.Email.IsEmailValid
                },
                Id = profile.Id,
                Index = profile.Index,
                IsActive = profile.IsActive,
                Name = new NameDto
                {
                    FirstName = profile.Name.FirstName,
                    LastName = profile.Name.LastName
                },
                PhoneNumber = profile.Phone.Number,
                Picture = new PictureDto
                {
                    IsExternal = profile.Picture.IsExternal,
                    Value = profile.Picture.Value
                },
                Registered = profile.Registered,
                Goods = new GoodsDto
                {
                    HousesCount = profile.Goods.Houses.Count,
                    VehiclesCount = profile.Goods.Vehicles.Count,
                    Vehicles = profile.Goods.Vehicles.Select(p => new VehicleDto
                    {
                        Model = p.Model,
                        Name = p.Name,
                        Type = p.Type.ToString(),
                        Year = p.Year
                    }).ToArray(),
                    Houses = profile.Goods.Houses.Select(p => new HouseDto
                    {
                        Address = new AddressDto
                        {
                            Id = p.Address.Id,
                            City = p.Address.City,
                            Country = p.Address.Country,
                            Street = p.Address.Street
                        },
                        AreaLength = p.Area.Length,
                        AreaWidth = p.Area.Width,
                        Price = p.Price,
                        RoomsCount = p.Rooms.Count,
                        Rooms = p.Rooms.Select(r => new RoomDto
                        {
                            AreaLength = r.Area.Length,
                            AreaWidth = r.Area.Width,
                            Name = r.Name
                        })
                    }).ToArray()
                }
            };

            dto.Tags = profile.Tags.Select(p => new TagDto
            {
                Name = p.Name,
                CategoryIsPublic = p.Category.IsPublic,
                CategoryName = p.Category.Name
            });

            dto.FavoriteMovies = profile.FavoriteMovies.Select(GetMovieDto).ToArray();
            return dto;
        }

        public static EmployeeDto GetEmployeeDto(Employee employee)
        {
            return new EmployeeDto(true)
            {
                Id = employee.Id,
                FullName = $"{employee.Name} {employee.Surname}",
                IsSociable = employee.Skills.Contains("Sociable"),
                Contact = $"Address: {employee.Address}, Phone: {employee.Phone}",
                Age = (int)Math.Floor((DateTime.Today - employee.BirthDate).TotalDays / 365.2425)
            };
        }
    }
}