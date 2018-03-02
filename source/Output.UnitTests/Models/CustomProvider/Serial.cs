using System.Text.RegularExpressions;

namespace Output.UnitTests.Models.CustomProvider
{
    public class Serial
    {
        public Serial(string serial)
        {
            if (Validate(serial))
                Value = serial;
        }

        public string Value { get; set; }

        public string FormattedValue
        {
            get
            {
                if (Value == null)
                    return null;

                return new Regex("(.{4})(.{6})(.{3})(.{3})").Replace(Value, "$1.$2-$3.$4");
            }
        }

        public static bool Validate(string serial)
        {
            return !string.IsNullOrWhiteSpace(serial) && serial.Length == 16;
        }
    }
}