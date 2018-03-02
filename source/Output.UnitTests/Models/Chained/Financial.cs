namespace Output.UnitTests.Models.Chained
{
    public class Financial
    {
        public Financial()
        {
            AccountNumber = "01234567";
            BankName = "World Bank";
            BankNumber = "001";
        }

        public string AccountNumber { get; set; }
        public string BankNumber { get; set; }
        public string BankName { get; set; }
    }
}