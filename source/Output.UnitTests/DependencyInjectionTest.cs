using Microsoft.Extensions.DependencyInjection;
using Output.UnitTests.Models.CustomProvider;
using Output.UnitTests.Models.Dependency;
using Output.UnitTests.Services;
using System;
using Xunit;

namespace Output.UnitTests
{
    public class DependencyInjectionTest
    {
        private ServiceProvider _serviceProvider;
        private readonly ServiceCollection _services;

        public DependencyInjectionTest()
        {
            _services = new ServiceCollection();
            _services.AddTransient<ISomeDataService, SomeDataService>();
        }

        [Fact]
        public void DependecyInjectionTest()
        {
            _services.AddOutputMapper();
            _serviceProvider = _services.BuildServiceProvider();

            var service = _serviceProvider.GetService<ISomeDataService>();
            var dto = service.GetSomeData();

            Assert.NotNull(dto);
            Assert.Equal(100, dto.Id);
            Assert.Equal("Almanaque", dto.Name);
            Assert.Equal(new DateTime(2019, 06, 22), dto.Date);
        }

        [Fact]
        public void DependencyInjectionWithOptionsTest()
        {
            _services.AddOutputMapper(options =>
            {
                options.AddConfig<SomeData, SomeDataDto>(config =>
                {
                    config.Map(p => p.Id, p => p.Id * 10);
                    config.Map(p => p.Name, p => p.Name.Substring(0, 4));
                    config.Ignore(p => p.Date);
                });
            });
            _serviceProvider = _services.BuildServiceProvider();

            var service = _serviceProvider.GetService<ISomeDataService>();
            var dto = service.GetSomeData();

            Assert.NotNull(dto);
            Assert.Equal(1000, dto.Id);
            Assert.Equal("Alma", dto.Name);
            Assert.Equal(DateTime.MinValue, dto.Date);
        }

        [Fact]
        public void DependencyInjectionWithCustomProviderTest()
        {
            _services.AddOutputMapper(new MyCustomMappingProvider());
            _serviceProvider = _services.BuildServiceProvider();

            var mapper = _serviceProvider.GetService<IMapper>();
            var source = new Product("MyProduct", new Serial("ABCDEFZZZ0001234"));
            var target = mapper.Map<ProductDto>(source);
            Assert.Equal(source.Name, target.Name);
            Assert.Equal(source.Serial.FormattedValue, target.Serial);
        }
    }
}