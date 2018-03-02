namespace Output.UnitTests.Models.Recursion
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public User User { get; set; }
    }

    public class PersonDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UserDto User { get; set; }
    }
}