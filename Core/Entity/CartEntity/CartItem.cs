using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.CartEntity
{
    public class CartItem:BaseEntity 
    {
       // public int Id { get; set; }
        [ForeignKey(nameof(Cart))]
        public int CartId { get; set; }
        [ForeignKey(nameof(Product))]
        public int productId { get; set; }  
        public int Quantity { get; set; }

        public Cart Cart { get; set; }  
        public Product Product { get; set; }
    }
}
