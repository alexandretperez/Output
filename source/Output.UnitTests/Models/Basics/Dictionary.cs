using System.Collections.Generic;

namespace Output.UnitTests.Models.Basics
{
    public class DictionaryModel
    {
        public DictionaryModel()
        {
            Keys = new Dictionary<byte, string>
            {
                { 1, "One" },
                { 2, "Two" }
            };

            Values = new Dictionary<string, byte>
            {
                { "Three", 3 },
                { "Four", 4 }
            };
        }

        public Dictionary<byte, string> Keys { get; set; }
        public Dictionary<string, byte> Values { get; set; }
    }

    public class DictionaryDto
    {
        public Dictionary<int, string> Keys { get; set; }
        public Dictionary<string, int> Values { get; set; }
    }
}