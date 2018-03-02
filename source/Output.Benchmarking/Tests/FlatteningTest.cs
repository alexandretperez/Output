using Output.Benchmarking.Models.Flattening;

namespace Output.Benchmarking.Tests
{
    public class FlatteningTest : TestBase<Earth, EarthDto>
    {
        protected override string GetName() => nameof(FlatteningTest);

        protected override Earth InitializeSource() => Earth.Create();
    }
}