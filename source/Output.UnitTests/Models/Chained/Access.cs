using System;

namespace Output.UnitTests.Models.Chained
{
    public class Access
    {
        public Access()
        {
            IsAuthorized = true;
            LastAccess = DateTime.Today;
            UserName = "fulanodetal";
        }

        public string UserName { get; set; }
        public bool IsAuthorized { get; set; }
        public DateTime LastAccess { get; set; }
    }
}