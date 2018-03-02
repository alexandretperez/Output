namespace Output.Benchmarking.Models.Intense
{
    public class Vehicle
    {
        public Vehicle(VehicleType type, string name, string model, int year)
        {
            Type = type;
            Name = name;
            Model = model;
            Year = year;
        }

        public VehicleType Type { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
    }

    public class VehicleDto
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
    }
}