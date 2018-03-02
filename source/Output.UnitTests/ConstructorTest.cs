using Output.Providers;
using Output.UnitTests.Models.Constructors;
using System;
using Xunit;

namespace Output.UnitTests
{
    public class ConstructorTest : TestBase<MappingProvider>
    {
        [Fact]
        public void SimpleConstructorTest()
        {
            // Arrange
            var model = new Sample(1, "Hello", true);

            // Act
            var dto = Mapper.Map<Sample>(model);

            // Assert
            Assert.Equal(model.Id, dto.Id);
            Assert.Equal(model.Name, dto.Name);
            Assert.Equal(model.IsActive, dto.IsActive);
        }

        [Fact]
        public void PrefixParameterSearchTest()
        {
            // Arrange
            var model = new Note();

            // Act
            var dto = Mapper.Map<NoteDto>(model);

            // Assert
            Assert.Equal(model.Id, dto.Id.ToString());
            Assert.Equal(new DateTime(model.DateYear, model.DateMonth, model.DateDay), dto.Date);
            Assert.Equal(model.Description, dto.Description);
        }


        [Fact]
        public void CustomMappingParameterResolverTest()
        {
            // Arrange
            var provider = new MappingProvider();
            provider.AddConfig<Product, ProductDto>(c =>
                c.Map(p => p.FullName, p => $"{p.Name} ({p.Serial})")
            );

            var mapper = new Mapper(provider);
            var model = new Product();

            // Act
            var dto = mapper.Map<ProductDto>(model);

            // Assert
            Assert.Equal(model.Id, dto.Id);
            Assert.Equal($"{model.Name} ({model.Serial})", dto.FullName);
        }


        [Fact]
        public void ComplexConstructorTest()
        {
            // Arrange
            var model = new SongDto();

            // Act
            var dto = Mapper.Map<Song>(model);

            // Assert
            Assert.Equal(model.Name, dto.Name);
            Assert.Equal(model.AlbumName, dto.Album.Name);
            Assert.Equal(new DateTime(model.AlbumReleaseDateYear, model.AlbumReleaseDateMonth, model.AlbumReleaseDateDay), dto.Album.ReleaseDate);
            Assert.Equal(model.AlbumComposerName, dto.Album.Composer.Name);
        }
    }
}