using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace Output.ProjectionTests.Database
{
    public class TestContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.AddFromAssembly(typeof(TestContext).Assembly);
        }
    }

    public class CustomerConfig : EntityTypeConfiguration<Customer>
    {
        public CustomerConfig()
        {
            ToTable("Customer");
            HasKey(p => p.Id);
        }
    }

    public class OrderConfig : EntityTypeConfiguration<Order>
    {
        public OrderConfig()
        {
            ToTable("Order");
            HasKey(p => p.Id);
            HasMany(p => p.Items);
            HasRequired(p => p.Customer).WithMany().HasForeignKey(p => p.CustomerId);
        }
    }

    public class OrderItemConfig : EntityTypeConfiguration<OrderItem>
    {
        public OrderItemConfig()
        {
            ToTable("OrderItem");
            HasKey(p => new { p.OrderId, p.ProductId });
            HasRequired(p => p.Order).WithMany(p => p.Items).HasForeignKey(p => p.OrderId);
            HasRequired(p => p.Product).WithMany().HasForeignKey(p => p.ProductId);
        }
    }

    public class ProductConfig : EntityTypeConfiguration<Product>
    {
        public ProductConfig()
        {
            ToTable("Product");
            HasKey(p => p.Id);
        }
    }
}