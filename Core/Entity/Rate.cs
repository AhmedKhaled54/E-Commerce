using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity
{
    public  class Rate:BaseEntity
    {
        public int?rate { get; set; }
        public string?Message { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }

        public User User { get; set; }  
        public Product Product { get; set; }    


    }

}
