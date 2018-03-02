using System;
using System.Collections.Generic;

namespace Output.ProjectionTests.Database
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime Date { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<OrderItem> Items { get; } = new List<OrderItem>();
    }

    public class OrderDto
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public CustomerDto Customer { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }

    public class OrderFlatDto
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public int ItemsCount { get; set; }
    }
}