using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.CartEntity
{
    public  class Cart:BaseEntity
    {
        //public int Id { get; set; }
        public List<CartItem> CartItems { get; set;}
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }  
        public User User { get; set; }  
    }
}
