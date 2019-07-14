using System;

namespace Output.UnitTests.Models.Dependency
{
    public class SomeData
    {
        public SomeData(int id, string name, DateTime date)
        {
            Id = id;
            Name = name;
            Date = date;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }

    public class SomeDataDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }
}