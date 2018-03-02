using Output.Benchmarking.Models.Custom;

namespace Output.Benchmarking.Tests
{
    public class CustomTest : TestBase<Employee, EmployeeDto>
    {
        protected override string GetName() => nameof(CustomTest);

        protected override Employee InitializeSource() => new Employee();
    }
}