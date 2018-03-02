using System.Collections.Generic;

namespace Output.UnitTests.Models.Recursion
{
    public class Pattern
    {
        public Pattern()
        {
            Colors = new List<Color> {
                new Color()
            };

            Colors[0].Patterns.Add(this);
        }

        public List<Color> Colors { get; set; }
    }

    public class PatternDto
    {
        public List<ColorDto> Colors { get; set; }
    }
}