using Output.Providers;
using Output.UnitTests.Models.Recursion;
using Xunit;

namespace Output.UnitTests
{
    public class CircularReferenceTest : TestBase<MappingProvider>
    {
        [Fact]
        public void CircularPropertyTest()
        {
            // Arrange
            var user = new User();

            // Act
            var dto = Mapper.Map<UserDto>(user);

            // Assert
            Assert.Equal(user.Email, dto.Email);
            Assert.Equal(user.Password, dto.Password);
            Assert.Equal(user.Person.Id, dto.Person.Id);
            Assert.Equal(user.Person.Name, dto.Person.Name);
            Assert.Equal(dto.Person, dto.Person.User.Person);
        }

        [Fact]
        public void CircularPropertyInCollectionTest()
        {
            // Arrange
            var model = new Pattern();

            // Act
            var dto = Mapper.Map<PatternDto>(model);

            // Assert
            Assert.Equal(model.Colors.Count, dto.Colors.Count);
            Assert.Equal(dto, dto.Colors[0].Patterns[0]);
        }
    }
}