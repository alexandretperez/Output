using System;

namespace Output.Benchmarking.Models.Intense
{
    public class Age
    {
        public Age(DateTime birthDate)
        {
            BirthDate = birthDate;
            Value = (int)Math.Floor((DateTime.Today - birthDate.Date).TotalDays / 365.2425);
        }

        public DateTime BirthDate { get; set; }
        public int Value { get; set; }
    }

    public class AgeDto
    {
        public DateTime BirthDate { get; set; }
        public int Value { get; set; }
    }
}