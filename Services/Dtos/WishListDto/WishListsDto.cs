using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.WishListDto
{
    public class WishListsDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Image {  get; set; }
        public string Description { get; set; }
        public decimal price { get; set; }


    }
}
