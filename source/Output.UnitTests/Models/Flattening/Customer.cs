using System;
using System.Collections.Generic;
using System.Drawing;

namespace Output.UnitTests.Models.Flattening
{
    public class Customer
    {
        public Customer()
        {
            Phones = new List<Phone>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Phone> Phones { get; set; }
        public Color CategoryColor { get; set; }
    }

    public class CustomerDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int PhonesCount { get; set; }
        public List<PhoneDto> Phones { get; set; }
        public byte CategoryColorR { get; set; }
        public byte CategoryColorG { get; set; }
        public byte CategoryColorB { get; set; }
    }
}