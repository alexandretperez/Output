namespace Output.UnitTests.Models.Basics
{
    public class Tiny
    {
        public Tiny(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class TinyDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}