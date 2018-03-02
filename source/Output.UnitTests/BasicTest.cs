using Output.Providers;
using Output.UnitTests.Models.Basics;
using System;
using System.Linq;
using Xunit;

namespace Output.UnitTests
{
    public class BasicTest : TestBase<MappingProvider>
    {
        [Fact]
        public void SimplePropertiesTest()
        {
            // Arrange
            var model = new Simple();

            // Act
            var dto = Mapper.Map<SimpleDto>(model);

            // Assert
            Assert.Equal(model.Boolean, dto.Boolean);
            Assert.Equal(model.Byte, dto.Byte);
            Assert.Equal(model.Char, dto.Char);
            Assert.Equal(model.DateTime, dto.DateTime);
            Assert.Equal(model.Decimal, dto.Decimal);
            Assert.Equal(model.Double, dto.Double);
            Assert.Equal(model.Float, dto.Float);
            Assert.Equal(model.Int, dto.Int);
            Assert.Equal(model.IntPtr, dto.IntPtr);
            Assert.Equal(model.Long, dto.Long);
            Assert.Equal(model.SByte, dto.SByte);
            Assert.Equal(model.Short, dto.Short);
            Assert.Equal(model.String, dto.String);
            Assert.Equal(model.UInt, dto.UInt);
            Assert.Equal(model.UIntPtr, dto.UIntPtr);
            Assert.Equal(model.ULong, dto.ULong);
            Assert.Equal(model.UShort, dto.UShort);
        }

        [Fact]
        public void NullTest()
        {
            Simple model = null;

            var dto = Mapper.Map<SimpleDto>(model);

            Assert.Null(dto);
        }


        [Fact]
        public void NullablePropertiesTest()
        {
            // Arrange
            var model = new SimpleNullable
            {
                Boolean = true,
                Decimal = 22.59m,
                IntPtr = new IntPtr(250),
                String = "Hello World!",
                UShort = 1
            };

            // Act
            var dto = Mapper.Map<SimpleDto>(model);

            // Assert Default
            Assert.Equal(default(byte), dto.Byte);
            Assert.Equal(default(char), dto.Char);
            Assert.Equal(default(DateTime), dto.DateTime);
            Assert.Equal(default(double), dto.Double);
            Assert.Equal(default(float), dto.Float);
            Assert.Equal(default(int), dto.Int);
            Assert.Equal(default(long), dto.Long);
            Assert.Equal(default(sbyte), dto.SByte);
            Assert.Equal(default(short), dto.Short);
            Assert.Equal(default(uint), dto.UInt);
            Assert.Equal(default(UIntPtr), dto.UIntPtr);
            Assert.Equal(default(ulong), dto.ULong);

            // Assert Initialized
            Assert.Equal(model.Boolean, dto.Boolean);
            Assert.Equal(model.Decimal, dto.Decimal);
            Assert.Equal(model.IntPtr, dto.IntPtr);
            Assert.Equal(model.String, dto.String);
            Assert.Equal(model.UShort, dto.UShort);
        }

        [Fact]
        public void CollectionConvertionTest()
        {
            // Arrange
            var model = new Collection();

            // Act
            var dto = Mapper.Map<CollectionDto>(model);

            // Assert
            Assert.NotNull(dto.IEnumerable_IQueryable);
            Assert.NotNull(dto.List_IEnumerable);
            Assert.NotNull(dto.IQueryable_Array);
            Assert.NotNull(dto.ICollection_HashSet);

            Assert.True(model.IEnumerable_IQueryable.SequenceEqual(dto.IEnumerable_IQueryable));
            Assert.True(model.List_IEnumerable.SequenceEqual(dto.List_IEnumerable));
            Assert.True(model.IQueryable_Array.SequenceEqual(dto.IQueryable_Array));
            Assert.True(model.ICollection_HashSet.SequenceEqual(dto.ICollection_HashSet));
        }

        [Fact]
        public void DictionaryConvertionTest()
        {
            // Arrange
            var model = new DictionaryModel();

            // Act
            var dto = Mapper.Map<DictionaryDto>(model);

            // Assert
            Assert.NotNull(dto.Keys);

            Assert.Collection(dto.Keys,
                kv => Assert.Equal(1, kv.Key),
                kv => Assert.Equal(2, kv.Key)
            );

            Assert.Collection(dto.Values,
                kv => Assert.Equal(3, kv.Value),
                kv => Assert.Equal(4, kv.Value)
            );
        }

        [Fact]
        public void GuidTest()
        {
            // Arrange
            var model = new GuidModel();

            // Act
            var dto = Mapper.Map<GuidDto>(model);

            // Assert
            Assert.Equal(model.Guid_String.ToString(), dto.Guid_String);
            Assert.Equal(model.String_Guid, dto.String_Guid.ToString());

            Assert.True(model.Guid_Bytes.ToByteArray().SequenceEqual(dto.Guid_Bytes));
            Assert.True(model.Bytes_Guid.SequenceEqual(dto.Bytes_Guid.ToByteArray()));
        }
    }
}