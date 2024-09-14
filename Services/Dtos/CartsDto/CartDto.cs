using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.CartsDto
{
    public  class CartDto
    {
        public int CartId { get; set; }
        public IEnumerable<CartItemDto> CartItems { get; set; }
    }
}
