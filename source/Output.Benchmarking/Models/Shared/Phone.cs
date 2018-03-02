namespace Output.Benchmarking.Models.Shared
{
    public class Phone
    {
        public Phone(string number, PhoneType type)
        {
            Number = number;
            Type = type;
        }

        public string Number { get; set; }
        public PhoneType Type { get; set; }
    }

    public class PhoneDto
    {
        public string Number { get; set; }
        public string Type { get; set; }
    }
}