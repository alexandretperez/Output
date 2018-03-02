namespace Output.UnitTests.Models.Flattening
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

    public enum PhoneType
    {
        Home,
        Work,
        Mobile,
        Fax
    }

    public class PhoneDto
    {
        public string Number { get; set; }
        public string Type { get; set; }
    }
}