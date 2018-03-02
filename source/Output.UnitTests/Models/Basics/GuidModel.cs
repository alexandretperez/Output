using System;

namespace Output.UnitTests.Models.Basics
{
    public class GuidModel
    {
        public GuidModel()
        {
            Guid_String = Guid.NewGuid();
            Guid_Bytes = Guid.NewGuid();
            String_Guid = Guid.NewGuid().ToString();
            Bytes_Guid = Guid.NewGuid().ToByteArray();
        }

        public Guid Guid_String { get; set; }
        public Guid Guid_Bytes { get; set; }
        public string String_Guid { get; set; }
        public byte[] Bytes_Guid { get; set; }
    }

    public class GuidDto
    {
        public string Guid_String { get; set; }
        public byte[] Guid_Bytes { get; set; }
        public Guid String_Guid { get; set; }
        public Guid Bytes_Guid { get; set; }
    }
}