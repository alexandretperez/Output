using System;

namespace Output.UnitTests.Models.Basics
{
    public class SimpleNullable
    {
        public static SimpleNullable InitializedInstance()
        {
            return new SimpleNullable()
            {
                Boolean = true,
                Byte = 1,
                SByte = 2,
                Short = 3,
                UShort = 4,
                Int = 5,
                UInt = 6,
                Long = 7,
                ULong = 8,
                IntPtr = new IntPtr(9),
                UIntPtr = new UIntPtr(10),
                Char = 'Z',
                Double = 11,
                Float = 12,
                String = "This is a very simple class",
                Decimal = 13,
                DateTime = System.DateTime.Now
            };
        }

        public bool? Boolean { get; set; }
        public byte? Byte { get; set; }
        public sbyte? SByte { get; set; }
        public short? Short { get; set; }
        public ushort? UShort { get; set; }
        public int? Int { get; set; }
        public uint? UInt { get; set; }
        public long? Long { get; set; }
        public ulong? ULong { get; set; }
        public IntPtr? IntPtr { get; set; }
        public UIntPtr? UIntPtr { get; set; }
        public char? Char { get; set; }
        public double? Double { get; set; }
        public float? Float { get; set; }
        public string String { get; set; }
        public decimal? Decimal { get; set; }
        public DateTime? DateTime { get; set; }
    }

    public class SimpleNullableDto
    {
        public bool? Boolean { get; set; }
        public byte? Byte { get; set; }
        public sbyte? SByte { get; set; }
        public short? Short { get; set; }
        public ushort? UShort { get; set; }
        public int? Int { get; set; }
        public uint? UInt { get; set; }
        public long? Long { get; set; }
        public ulong? ULong { get; set; }
        public IntPtr? IntPtr { get; set; }
        public UIntPtr? UIntPtr { get; set; }
        public char? Char { get; set; }
        public double? Double { get; set; }
        public float? Float { get; set; }
        public string String { get; set; }
        public decimal? Decimal { get; set; }
        public DateTime? DateTime { get; set; }
    }
}