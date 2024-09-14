using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.OrderDto
{
    public  class OrderItemDto
    {
        public int ProductId { get; set; }
        public int quantity { get; set; }
        public decimal price { get; set; }

    }
}
