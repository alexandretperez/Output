using Output.Benchmarking.Models.Basic;

namespace Output.Benchmarking.Tests
{
    public class BasicTest : TestBase<Customer, CustomerDto>
    {
        protected override string GetName() => nameof(BasicTest);

        protected override Customer InitializeSource() => new Customer();
    }
}