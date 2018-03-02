using Output.ProjectionTests.Database;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Output.ProjectionTests
{
    [Collection("Db")]
    public class ProjectionTest : TestBase
    {
        private List<OrderFlatDto> _dtoFlat;
        private List<OrderDto> _dtoAssoc;

        public ProjectionTest(Tests.DbFixture fixture, ITestOutputHelper xunitLog) : base(fixture, xunitLog)
        {
        }

        protected override void Run()
        {
            CaptureSql(() =>
            {
                _dtoFlat = Mapper.Project<OrderFlatDto>(Db.Set<Order>()).ToList();
                _dtoAssoc = Mapper.Project<OrderDto>(Db.Set<Order>()).ToList();
            });
        }

        [Fact]
        public void FlatteningTest()
        {
            var order = Db.Set<Order>().First();

            var dto = _dtoFlat[0];
            Assert.Equal(order.Id, dto.Id);
            Assert.Equal(order.CustomerId, dto.CustomerId);
            Assert.Equal(order.Customer.Name, dto.CustomerName);
            Assert.Equal(order.Customer.Email, dto.CustomerEmail);
            Assert.Equal(order.Date.ToString("yyyy-MM-dd HH:mm:ss"), dto.Date.ToString("yyyy-MM-dd HH:mm:ss"));
            Assert.Equal(order.Items.Count, dto.ItemsCount);
        }

        [Fact]
        public void AssociationTest()
        {
            var order = Db.Set<Order>().First();

            var dto = _dtoAssoc[0];
            Assert.Equal(order.Id, dto.Id);
            Assert.Equal(order.CustomerId, dto.Customer.Id);
            Assert.Equal(order.Customer.Name, dto.Customer.Name);
            Assert.Equal(order.Customer.Email, dto.Customer.Email);
            Assert.Equal(order.Date.ToString("yyyy-MM-dd HH:mm:ss"), dto.Date.ToString("yyyy-MM-dd HH:mm:ss"));

            var orderItem = order.Items.OrderBy(p => p.ProductId).ToArray();
            Assert.Collection(dto.Items.OrderBy(p => p.ProductId),
                item =>
                {
                    Assert.Equal(orderItem[0].ProductId, item.ProductId);
                    Assert.Equal(orderItem[0].Amount, item.Amount);
                    Assert.Equal(orderItem[0].UnitPrice, item.UnitPrice);
                },
                item =>
                {
                    Assert.Equal(orderItem[1].ProductId, item.ProductId);
                    Assert.Equal(orderItem[1].Amount, item.Amount);
                    Assert.Equal(orderItem[1].UnitPrice, item.UnitPrice);
                }
            );
        }
    }
}