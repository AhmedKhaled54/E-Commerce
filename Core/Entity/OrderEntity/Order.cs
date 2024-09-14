using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.OrderEntity
{
    public  class Order:BaseEntity
    {
       // public int Id { get; set; }
        public DateTime CreatedAt { get; set; }= DateTime.Now;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public ShippingAddress ShippingAddress { get; set; }
        public IReadOnlyList<OrderItem> OrderItems { get; set; }
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public User User { get; set; }
        public decimal Total { get; set; }
    }
}
