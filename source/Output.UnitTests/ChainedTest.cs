using Output.Extensions;
using Output.Providers;
using Output.UnitTests.Models.Chained;
using System;
using System.Collections.Generic;
using Xunit;

namespace Output.UnitTests
{
    public class ChainedTest : TestBase<MappingProvider>
    {
        [Fact]
        public void MapChainTest()
        {
            // Arrange
            var user = new User();
            var access = new Access();
            var financial = new Financial();

            // Act
            var dto = new AllDataDto();
            Mapper.Map(user, dto);
            Mapper.Map(access, dto);
            Mapper.Map(financial, dto);

            // Assert
            Assert.Equal(user.Id, dto.Id);
            Assert.Equal(user.Email, dto.Email);
            Assert.Equal(user.Registration, dto.Registration);
            Assert.Equal(user.Name, dto.Name);
            Assert.Equal(access.IsAuthorized, dto.IsAuthorized);
            Assert.Equal(access.LastAccess, dto.LastAccess);
            Assert.Equal(access.UserName, dto.UserName);
            Assert.Equal(financial.AccountNumber, dto.AccountNumber);
            Assert.Equal(financial.BankName, dto.BankName);
            Assert.Equal(financial.BankNumber, dto.BankNumber);
        }

        [Fact]
        public void MapChainAlternativeTest()
        {
            // Arrange
            var user = new User();
            var access = new Access();
            var financial = new Financial();

            // Act
            var dto = new AllDataDto();
            Mapper.MapChain(dto, user, access, financial);

            // Assert
            Assert.Equal(user.Id, dto.Id);
            Assert.Equal(user.Email, dto.Email);
            Assert.Equal(user.Registration, dto.Registration);
            Assert.Equal(user.Name, dto.Name);
            Assert.Equal(access.IsAuthorized, dto.IsAuthorized);
            Assert.Equal(access.LastAccess, dto.LastAccess);
            Assert.Equal(access.UserName, dto.UserName);
            Assert.Equal(financial.AccountNumber, dto.AccountNumber);
            Assert.Equal(financial.BankName, dto.BankName);
            Assert.Equal(financial.BankNumber, dto.BankNumber);
        }
    }
}