using System;

namespace Output.UnitTests.Models.Flattening
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime HireDate { get; set; }
        public Address Address { get; set; }
    }

    public class Address
    {
        public Address(string street, string city)
        {
            Street = street;
            City = city;
        }

        public string Street { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }

    public class EmployeeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int HireDateDay { get; set; }
        public int HireDateMonth { get; set; }
        public int HireDateYear { get; set; }
        public string AddressStreet { get; set; }
        public string AddressCity { get; set; }
        public string AddressCountry { get; set; }
        public string AddressPostalCode { get; set; }
    }
}