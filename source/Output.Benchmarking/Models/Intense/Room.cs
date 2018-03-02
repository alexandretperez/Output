namespace Output.Benchmarking.Models.Intense
{
    public class Room
    {
        public Room(string name, Area area)
        {
            Area = area;
            Name = name;
        }

        public Area Area { get; set; }

        public string Name { get; set; }
    }

    public class RoomDto
    {
        public double AreaLength { get; set; }
        public double AreaWidth { get; set; }
        public string Name { get; set; }
    }
}