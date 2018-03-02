using System;

namespace Output.ProjectionTests.Database
{
    public class OrderItem
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Amount { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }

    public class OrderItemDto
    {
        public Guid ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Amount { get; set; }
    }
}