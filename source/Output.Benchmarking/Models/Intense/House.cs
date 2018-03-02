using Output.Benchmarking.Models.Shared;
using System.Collections.Generic;

namespace Output.Benchmarking.Models.Intense
{
    public class House
    {
        public House()
        {
            Area = new Area(220.50, 180.75);
            Address = new Address(100, "One Street", "SimCity", "Steam");
            Price = 500000.00m;
            Rooms = new List<Room>
        {
            new Room("Dining", new Area(10.6, 7.5)),
            new Room("Kitchen", new Area(10.6, 9.3)),
            new Room("Laundry", new Area(5.2, 6.8)),
            new Room("Storage", new Area(5.0, 3.6)),
            new Room("Bedroom #1", new Area(10.0, 9.0)),
            new Room("Bedroom #2", new Area(11.3, 10.6)),
            new Room("Bath",new Area(5.0,8.8)),
            new Room("Master", new Area(12.5, 10.2)),
            new Room("Master Bath", new Area(6.2,8.0)),
            new Room("Living", new Area(15.4,13.7))
        };
        }

        public Area Area { get; set; }
        public Address Address { get; set; }
        public List<Room> Rooms { get; set; }
        public decimal Price { get; set; }
    }

    public class HouseDto
    {
        public double AreaLength { get; set; }
        public double AreaWidth { get; set; }
        public AddressDto Address { get; set; }
        public IEnumerable<RoomDto> Rooms { get; set; }
        public decimal Price { get; set; }
        public int RoomsCount { get; set; }
    }
}