using System;

namespace Output.UnitTests.Models.Basics
{
    public class Enum
    {
        public Enum()
        {
            Enum_EnumDto = DayOfWeek.Wednesday;
            Enum_Number = DayOfWeek.Saturday;
            Enum_String = DayOfWeek.Friday;
            String_Enum = "Monday";
            Number_Enum = 2;
        }

        public DayOfWeek Enum_EnumDto { get; set; }
        public DayOfWeek Enum_Number { get; set; }
        public DayOfWeek Enum_String { get; set; }
        public string String_Enum { get; set; }
        public byte Number_Enum { get; set; }
    }

    public enum DayOfWeekDto
    {
        Sunday = 0,
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6
    }

    public class EnumDto
    {
        public DayOfWeekDto Enum_EnumDto { get; set; }
        public int Enum_Number { get; set; }
        public string Enum_String { get; set; }
        public DayOfWeek String_Enum { get; set; }
        public DayOfWeek Number_Enum { get; set; }
    }
}