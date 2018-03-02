using Output.ProjectionTests.Database;
using System;

namespace Output.ProjectionTests.Tests
{
    public class DbFixture : IDisposable
    {
        public TestContext Db { get; }

        public DbFixture()
        {
            Db = new TestContext();
            Db.Database.Delete();
            Db.Database.Create();
            Seed();
        }

        public void Dispose()
        {
            Db.Database.Delete();
            Db.Dispose();
        }

        private void Seed()
        {
            var products = new[]{
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Product A"
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Product B"
                }
            };

            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                Email = "fulanodetal@none.com",
                Name = "Fulado de Tal"
            };

            var order = new Order
            {
                Id = Guid.NewGuid(),
                Date = DateTime.Now,
                CustomerId = customer.Id
            };

            order.Items.Add(new OrderItem
            {
                ProductId = products[0].Id,
                Amount = 5,
                UnitPrice = 10.50m
            });

            order.Items.Add(new OrderItem
            {
                ProductId = products[1].Id,
                Amount = 10,
                UnitPrice = 7.20m
            });

            Db.Set<Customer>().Add(customer);
            Db.Set<Product>().AddRange(products);
            Db.Set<Order>().Add(order);
            Db.SaveChanges();
        }
    }
}