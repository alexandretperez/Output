using System;

namespace Output.Benchmarking.Models.Intense
{
    public class Picture
    {
        public Picture(string value)
        {
            Value = value;
            IsExternal = value.StartsWith("http://", StringComparison.OrdinalIgnoreCase);
        }

        public string Value { get; set; }
        public bool IsExternal { get; set; }
    }

    public class PictureDto
    {
        public string Value { get; set; }
        public bool IsExternal { get; set; }
    }
}