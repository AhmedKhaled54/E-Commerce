using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.OrderEntity
{
    public  class OrderItem:BaseEntity
    {
        //public int Id { get; set; }
        public int quantity { get; set; }   
        public decimal UnitPrice { get; set; }
        public decimal SupTotal=>quantity*UnitPrice;

        [ForeignKey(nameof(Product))]
        public int productId { get; set; }
        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }

        public Order Order { get; set; }
        public Product Product { get; set; }


    }
}
