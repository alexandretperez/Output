namespace Output.Benchmarking.Models.Intense
{
    public class Name
    {
        public Name(string first, string last)
        {
            FirstName = first;
            LastName = last;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class NameDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}