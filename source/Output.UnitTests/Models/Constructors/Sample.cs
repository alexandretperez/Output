namespace Output.UnitTests.Models.Constructors
{
    public class Sample
    {
        public Sample(int id, string name, bool isActive)
        {
            Id = id;
            Name = name;
            IsActive = isActive;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }

    public class SampleDto
    {
        public SampleDto(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}