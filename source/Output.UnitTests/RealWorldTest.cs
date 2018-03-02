using Output.Providers;
using Output.UnitTests.Models.RealWorld;
using System.Linq;
using Xunit;

namespace Output.UnitTests
{
    public class RealWorldTest : TestBase<MappingProvider>
    {
        [Fact]
        public void MovieTest()
        {
            // Arrange
            var model = new Movie();

            // Act
            var dto = Mapper.Map<MovieDto>(model);

            // Assert
            Assert.Equal(model.Name, dto.Name);
            Assert.Equal(model.Director.Name, dto.Director.Name);

            Assert.True(model.Genres.Select(p => p.Name).SequenceEqual(dto.Genres.Select(p => p.Name)));
            Assert.True(model.Actors.Select(p => p.Name).SequenceEqual(dto.Actors.Select(p => p.Name)));

            Assert.Equal(dto, dto.Director.Movies[0]);
        }
    }
}