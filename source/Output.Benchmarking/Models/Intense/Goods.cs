using System.Collections.Generic;

namespace Output.Benchmarking.Models.Intense
{
    public class Goods
    {
        public Goods()
        {
            Houses = new List<House>();
            Vehicles = new List<Vehicle>();
        }

        public List<Vehicle> Vehicles { get; set; }
        public List<House> Houses { get; set; }
    }

    public class GoodsDto
    {
        public int VehiclesCount { get; set; }
        public int HousesCount { get; set; }
        public VehicleDto[] Vehicles { get; set; }
        public HouseDto[] Houses { get; set; }
    }
}