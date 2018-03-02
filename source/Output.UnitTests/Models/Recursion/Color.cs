using System.Collections.Generic;

namespace Output.UnitTests.Models.Recursion
{
    public class Color
    {
        public Color()
        {
            Patterns = new List<Pattern>();
        }

        public List<Pattern> Patterns { get; set; }
    }

    public class ColorDto
    {
        public List<PatternDto> Patterns { get; set; }
    }
}