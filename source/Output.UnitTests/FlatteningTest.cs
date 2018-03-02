using Output.Providers;
using Output.UnitTests.Models.Flattening;
using System;
using System.Collections.Generic;
using Xunit;

namespace Output.UnitTests
{
    public class FlatteningTest : TestBase<MappingProvider>
    {
        [Fact]
        public void CollectionCount()
        {
            // Arrange
            var model = new Customer
            {
                Id = Guid.NewGuid(),
                Name = "John Connor",
                Phones = new List<Phone>
                {
                    new Phone("01 1 2345-6789", PhoneType.Home),
                    new Phone("09 8 7654-3210", PhoneType.Mobile),
                },
                CategoryColor = System.Drawing.Color.FromArgb(255, 150, 75)
            };

            // Act
            var dto = Mapper.Map<CustomerDto>(model);

            // Assert
            Assert.Equal(model.Id.ToString(), dto.Id);
            Assert.Equal(model.Name, dto.Name);
            Assert.Equal(2, dto.PhonesCount);
            Assert.Equal(model.CategoryColor.R, dto.CategoryColorR);
            Assert.Equal(model.CategoryColor.G, dto.CategoryColorG);
            Assert.Equal(model.CategoryColor.B, dto.CategoryColorB);
            Assert.Collection(dto.Phones,
                home =>
                {
                    Assert.Equal("01 1 2345-6789", home.Number);
                    Assert.Equal("Home", home.Type);
                },
                mobile =>
                {
                    Assert.Equal("09 8 7654-3210", mobile.Number);
                    Assert.Equal("Mobile", mobile.Type);
                }
            );
        }

        [Fact]
        public void FlatteningLongestWordTest()
        {
            // Arrange
            var model = LongestWord.Create();

            // Act
            var dto = Mapper.Map<LongestWordDto>(model);

            // Assert
            Assert.Equal(model.Pneumono.Ultra.Microscopic.Silico.Volcano.Coniosis.Author, dto.PneumonoUltraMicroscopicSilicoVolcanoConiosisAuthor);
            Assert.Equal(model.Pneumono.Ultra.Microscopic.Silico.Volcano.Coniosis.PublishedDate, dto.PneumonoUltraMicroscopicSilicoVolcanoConiosisPublishedDate);
        }

        [Fact]
        public void FlatteningIncompleteLongestWordTest()
        {
            // Arrange
            var model = LongestWord.Create();
            model.Pneumono.Ultra.Microscopic = null;

            // Act
            var dto = Mapper.Map<LongestWordDto>(model);

            // Assert
            Assert.Null(dto.PneumonoUltraMicroscopicSilicoVolcanoConiosisAuthor);
            Assert.Equal(DateTime.MinValue, dto.PneumonoUltraMicroscopicSilicoVolcanoConiosisPublishedDate);
        }

        [Fact]
        public void UnflatteningTheLongestWordTest()
        {
            // Arrange
            var dto = new LongestWordDto()
            {
                PneumonoUltraMicroscopicSilicoVolcanoConiosisAuthor = "Everett M. Smith",
                PneumonoUltraMicroscopicSilicoVolcanoConiosisPublishedDate = new DateTime(1935, 02, 23)
            };

            // Act
            var model = Mapper.Map<LongestWord>(dto);

            // Assert
            Assert.Equal(dto.PneumonoUltraMicroscopicSilicoVolcanoConiosisAuthor, model.Pneumono.Ultra.Microscopic.Silico.Volcano.Coniosis.Author);
            Assert.Equal(dto.PneumonoUltraMicroscopicSilicoVolcanoConiosisPublishedDate, model.Pneumono.Ultra.Microscopic.Silico.Volcano.Coniosis.PublishedDate);
        }

        [Fact]
        public void UnflatteningWithoutParameterlessConstructorTest()
        {
            // Arrange
            var dto = new EmployeeDto
            {
                AddressCity = "Curitiba",
                AddressCountry = "Brasil",
                AddressPostalCode = "00.123-456",
                AddressStreet = "Principal",
                HireDateDay = 1,
                HireDateMonth = 2,
                HireDateYear = 2005,
                Name = "Fulano de Tal",
                Id = Guid.NewGuid()
            };

            // Act
            var entity = Mapper.Map<Employee>(dto);

            // Assert
            Assert.Equal(dto.AddressCity, entity.Address.City);
            Assert.Equal(dto.AddressCountry, entity.Address.Country);
            Assert.Equal(dto.AddressPostalCode, entity.Address.PostalCode);
            Assert.Equal(dto.AddressStreet, entity.Address.Street);
            Assert.Equal(new DateTime(2005, 2, 1).Date, entity.HireDate.Date);
            Assert.Equal(dto.Name, entity.Name);
            Assert.Equal(dto.Id, entity.Id);
        }
    }
}