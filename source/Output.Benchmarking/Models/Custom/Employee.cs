using System;
using System.Collections.Generic;

namespace Output.Benchmarking.Models.Custom
{
    public class Employee
    {
        public Employee()
        {
            Id = 1;
            Name = "Meninão";
            Surname = "Pinpolho";
            Skills = new[] {
                "Sociable",
                "Tenacious",
                "Amusing"
            };
            Email = "meninao.pinpolho@none.com";
            Phone = "+1 123-4567-89";
            BirthDate = new DateTime(1983, 7, 30);
            Address = "0123 Street One, Earth";
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public IEnumerable<string> Skills { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }
    }

    public class EmployeeDto
    {
        public EmployeeDto(bool active)
        {
            IsActive = active;
        }

        public int Id { get; set; }
        public string FullName { get; set; }
        public bool IsSociable { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; }
        public string Contact { get; set; }
        public int Age { get; set; }
    }
}