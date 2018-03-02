using Output.UnitTests.Models.CustomProvider;
using Xunit;

namespace Output.UnitTests
{
    public class CustomProviderTest
    {
        private readonly IMapper _mapper;

        public CustomProviderTest()
        {
            _mapper = new Mapper(new MyCustomMappingProvider());
        }

        [Fact]
        public void MapCustomType()
        {
            // Arrange
            var model = new Product("Product 1", new Serial("ACNKSJUDI8492859"));

            // Act
            var dto = _mapper.Map<ProductDto>(model);

            // Assert
            Assert.Equal(model.Name, dto.Name);
            Assert.Equal(model.Serial.FormattedValue, dto.Serial);
        }
    }
}