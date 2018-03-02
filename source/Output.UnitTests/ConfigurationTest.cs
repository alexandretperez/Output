using Output.Configurations;
using Output.Providers;
using Output.UnitTests.Models.Configuration;
using System;
using Xunit;

namespace Output.UnitTests
{
    public class ConfigurationTest
    {
        private readonly MappingProvider _mappingProvider;

        public ConfigurationTest()
        {
            _mappingProvider = new MappingProvider();
            _mappingProvider
                .AddConfig(GetCustomerConfiguration());
        }

        [Fact]
        public void CustomMappingTest()
        {
            // Arrange
            var mapper = new Mapper(_mappingProvider);
            var model = new Customer();

            // Act
            var dto = mapper.Map<CustomerDto>(model);

            // Assert
            Assert.Equal(10, dto.Id);
            Assert.Equal("Joana D'arc", dto.Name);
            Assert.Equal("jojo@arc.com", dto.Email);
            Assert.Null(dto.Status);
        }

        [Fact]
        public void CustomMappingNullPropagationTest()
        {
            // Arrange
            var mapper = new Mapper(_mappingProvider);
            var model = new Customer()
            {
                Email = null
            };

            // Act
            var dto = mapper.Map<CustomerDto>(model);

            // Assert
            Assert.Null(dto.Email);
        }

        [Fact]
        public void CustomMappingConfigAlreadyExistsExceptionTest()
        {
            Assert.Throws<Exception>(() => _mappingProvider.AddConfig<Customer, CustomerDto>(config => config.Ignore(p => p.Name)));
        }

        private static IMappingConfiguration GetCustomerConfiguration()
        {
            return new MappingConfiguration<Customer, CustomerDto>()
                .Map(p => p.Name, p => $"{p.FirstName} {p.LastName}")
                .Map(p => p.Email, p => p.Email.Value)
                .Map(p => p.Status, p => "Teste")
                .Ignore(p => p.Status);
        }
    }
}