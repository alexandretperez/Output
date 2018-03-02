using Output.Benchmarking.Models.Complex;

namespace Output.Benchmarking.Tests
{
    public class ComplexTest : TestBase<Movie, MovieDto>
    {
        protected override string GetName() => nameof(ComplexTest);

        protected override Movie InitializeSource() => Movie.LordOfTheRings();
    }
}