using System;

namespace Output.UnitTests.Models.Chained
{
    public class User
    {
        public User()
        {
            Id = Guid.NewGuid();
            Email = "email@email.com";
            Name = "Fulano";
            Registration = new DateTime(2017, 1, 2);
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime Registration { get; set; }
    }

    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}