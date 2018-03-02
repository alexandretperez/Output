namespace Output.UnitTests.Models.Recursion
{
    public class User
    {
        public User()
        {
            Person = new Person
            {
                Id = 1,
                Name = "Peter Parker",
                User = this
            };

            Email = "spider@man.com";
            Password = "123456";
        }

        public string Email { get; set; }
        public string Password { get; set; }
        public Person Person { get; set; }
    }

    public class UserDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public PersonDto Person { get; set; }
    }
}