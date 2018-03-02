using Output.Providers;
using Output.UnitTests.Models.Basics;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Output.UnitTests
{
    public class CollectionTest : TestBase<MappingProvider>
    {
        [Fact]
        public void ListToArrayTest()
        {
            // Arrange
            var model = new List<Tiny> {
                new Tiny(1, "A"),
                new Tiny(2, "B")
            };

            // Act
            var dto = Mapper.Map<TinyDto[]>(model);

            Assert.IsType<TinyDto[]>(dto);

            // Assert
            Assert.Collection(dto,
                a =>
                {
                    Assert.Equal(1, a.Id);
                    Assert.Equal("A", a.Name);
                },
                b =>
                {
                    Assert.Equal(2, b.Id);
                    Assert.Equal("B", b.Name);
                }
            );
        }

        [Fact]
        public void DictionaryValueMapTest()
        {
            // Arrange
            var model = new Dictionary<int, Tiny> {
                { 1, new Tiny(1, "A") },
                { 2, new Tiny(1, "A") }
            };

            // Act
            var dto = Mapper.Map<Dictionary<int, TinyDto>>(model);

            // Assert
            foreach (var k in model.Keys)
            {
                Assert.Equal(model[k].Id, dto[k].Id);
                Assert.Equal(model[k].Name, dto[k].Name);
            }
        }

        [Fact]
        public void DictionaryKeyMapTest()
        {
            // Arrange
            var model = new Dictionary<Tiny, int> {
                { new Tiny(1, "A"), 1 },
                { new Tiny(1, "A"), 2 }
            };

            // Act
            var dto = Mapper.Map<Dictionary<TinyDto, int>>(model);

            // Assert
            var modelKeys = new List<Tiny>();
            var dtoKeys = new List<TinyDto>();

            foreach (var mk in model.Keys)
                modelKeys.Add(mk);

            foreach (var vk in dto.Keys)
                dtoKeys.Add(vk);

            for (int i = 0; i < 2; i++)
            {
                var m = modelKeys[i];
                var v = dtoKeys[i];

                Assert.Equal(m.Id, v.Id);
                Assert.Equal(m.Name, v.Name);
                Assert.Equal(model[m], dto[v]);
            }
        }

        [Fact]
        public void IEnumerableToHashSetTest()
        {
            var a = new DateTime(2017, 12, 26);
            var b = new DateTime(2017, 12, 31);

            // Arrange
            var model = new[] { a, b, a, a, b }.AsEnumerable();

            // Act
            var dto = Mapper.Map<HashSet<DateTime>>(model);

            // Assert
            Assert.Equal(2, dto.Count);
            Assert.Collection(dto,
                one => Assert.Equal(a, one),
                two => Assert.Equal(b, two)
            );
        }
    }
}