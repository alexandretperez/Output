using System;

namespace Output.UnitTests.Models.Chained
{
    public class AllDataDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime Registration { get; set; }

        public string AccountNumber { get; set; }
        public string BankNumber { get; set; }
        public string BankName { get; set; }

        public string UserName { get; set; }
        public bool IsAuthorized { get; set; }
        public DateTime LastAccess { get; set; }
    }
}