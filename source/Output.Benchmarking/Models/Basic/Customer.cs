using Output.Benchmarking.Models.Shared;
using System.Collections.Generic;

namespace Output.Benchmarking.Models.Basic
{
    public class Customer
    {
        public Customer()
        {
            Id = 1;
            Name = "Fulano de Tal";
            IsActive = true;
            HomeAddress = new Address(1, "First Street", "First City", "First Contry");
            WorkAddresses = new List<Address>
            {
                new Address(2, "Second Street", "Second City", "Second Contry"),
                new Address(3, "Third Street", "Third City", "Third Contry")
            };
            Phones = new List<Phone>
            {
                new Phone("01 2 3456-7890", PhoneType.Mobile),
                new Phone("98 7 6543-2109", PhoneType.Home)
            };
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public Address HomeAddress { get; set; }
        public List<Address> WorkAddresses { get; set; }
        public List<Phone> Phones { get; set; }
    }

    public class CustomerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public AddressDto HomeAddress { get; set; }
        public List<AddressDto> WorkAddresses { get; set; }
        public int WorkAddressesCount { get; set; }
        public List<PhoneDto> Phones { get; set; }
        public int PhonesCount { get; set; }
    }
}