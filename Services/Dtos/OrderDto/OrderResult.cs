using Core.Entity.OrderEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.OrderDto
{
    public  class OrderResult
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public IReadOnlyList<OrderItemDto> Items { get; set; }
        public OrderStatus Status { get; set; }
        public AddressDto ShippingAddress   { get; set; }
        public decimal SupTotal  { get; set; }

    }
}
