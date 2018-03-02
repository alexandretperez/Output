namespace Output.Benchmarking.Models.Intense
{
    public class Email
    {
        public Email(string value)
        {
            Value = value;
            IsEmailValid = value.IndexOf("@") > -1; // naive
        }

        public string Value { get; set; }
        public bool IsEmailValid { get; set; }
    }

    public class EmailDto
    {
        public string Value { get; set; }
        public bool IsEmailValid { get; set; }
    }
}