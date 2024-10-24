using Core.Entity.OrderEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity
{
    public  class Payment:BaseEntity
    {
       
        [ForeignKey(nameof(Order))]
        public int OrderId  { get; set; }   
        public Order Order { get; set; }    
        public string Method {  get; set; }
        public string Currency { get; set; }
        public decimal  Amount { get; set; } 
        public string PaymentStstus { get; set; }   
        public string PaymentIntentId { get; set; }
        public DateTime CreatedAt { get; set; }= DateTime.Now;

    }
}
