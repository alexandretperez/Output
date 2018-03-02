using System;

namespace Output.UnitTests.Models.Constructors
{
    public class Product
    {
        public Product()
        {
            Id = Guid.NewGuid();
            Serial = "XYZ-1234";
            Name = "Keyboard";
        }

        public Guid Id { get; set; }
        public string Serial { get; set; }
        public string Name { get; set; }
    }

    public class ProductDto
    {
        public ProductDto(Guid id, string fullName)
        {
            Id = id;
            FullName = fullName;
        }

        public Guid Id { get; set; }
        public string FullName { get; set; }
    }
}