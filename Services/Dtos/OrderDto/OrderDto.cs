using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.OrderDto
{
    public  class OrderDto
    {
        ///public string UsertId { get; set; } 
        public AddressDto ShippingAddress { get; set; }
    }
}
