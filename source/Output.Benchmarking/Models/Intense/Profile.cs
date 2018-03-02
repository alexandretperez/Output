using Output.Benchmarking.Models.Complex;
using Output.Benchmarking.Models.Shared;
using System;
using System.Collections.Generic;

namespace Output.Benchmarking.Models.Intense
{
    public class Profile
    {
        public Profile(Guid id, Name name, Age age)
        {
            Id = id;
            Name = name;
            Age = age;
            Company = "Company 1";
            Index = 1;
            IsActive = true;
            Picture = new Picture("http://via.placeholder.com/350x150");
            Balance = 15000;
            Email = new Email("some@some.com");
            Phone = new Phone("+1 55 5551-5552", PhoneType.Home);
            About = "Ad nostrud minim irure dolor ad eiusmod tempor ipsum eiusmod cillum ex. Exercitation aute est fugiat nulla pariatur commodo cupidatat esse elit laboris. Enim labore pariatur nulla sunt.";
            Tags = new[]
            {
                new Tag("A", new TagCategory("Admin", false)),
                new Tag("B", new TagCategory("Public", true))
            };
            Goods = new Goods();
            Goods.Houses.Add(new House());
            Goods.Vehicles.AddRange(new[] {
                new Vehicle(VehicleType.Car, "Ice", "Neptune", 2050),
                new Vehicle(VehicleType.Motorcycle, "Fire", "Sun", 2051),
                new Vehicle(VehicleType.Truck, "Rock", "Earth", 2052)
            });
            Registered = new DateTime(1994, 10, 21);
            FavoriteMovies = new List<Movie>();
        }

        public Guid Id { get; set; }
        public int Index { get; set; }
        public bool IsActive { get; set; }
        public decimal Balance { get; set; }
        public Picture Picture { get; set; }
        public Age Age { get; set; }
        public Name Name { get; set; }
        public string Company { get; set; }
        public Email Email { get; set; }
        public Phone Phone { get; set; }
        public string About { get; set; }
        public DateTime Registered { get; set; }
        public Tag[] Tags { get; set; }
        public Goods Goods { get; set; }
        public List<Movie> FavoriteMovies { get; set; }
    }

    public class ProfileDto
    {
        public Guid Id { get; set; }
        public int Index { get; set; }
        public bool IsActive { get; set; }
        public decimal Balance { get; set; }
        public PictureDto Picture { get; set; }
        public AgeDto Age { get; set; }
        public NameDto Name { get; set; }
        public string Company { get; set; }
        public EmailDto Email { get; set; }
        public string PhoneNumber { get; set; }
        public string About { get; set; }
        public DateTime Registered { get; set; }
        public IEnumerable<TagDto> Tags { get; set; }
        public GoodsDto Goods { get; set; }
        public MovieDto[] FavoriteMovies { get; set; }
    }
}