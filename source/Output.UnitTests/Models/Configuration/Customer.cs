namespace Output.UnitTests.Models.Configuration
{
    public class Customer
    {
        public Customer()
        {
            Id = 10;
            FirstName = "Joana";
            LastName = "D'arc";
            Email = new Email("jojo@arc.com");
            Status = CustomerStatus.Premium;
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Email Email { get; set; }
        public CustomerStatus Status { get; set; }
    }

    public class CustomerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
    }
}