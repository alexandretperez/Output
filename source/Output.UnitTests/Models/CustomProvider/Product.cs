namespace Output.UnitTests.Models.CustomProvider
{
    public class Product
    {
        public Product(string name, Serial serial)
        {
            Name = name;
            Serial = serial;
        }

        public string Name { get; set; }
        public Serial Serial { get; set; }
    }

    public class ProductDto
    {
        public string Name { get; set; }
        public string Serial { get; set; }
    }
}