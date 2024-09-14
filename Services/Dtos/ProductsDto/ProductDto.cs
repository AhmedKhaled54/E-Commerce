using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.ProductsDto
{
    public  class ProductDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal price { get; set; }  
        public string Image {  get; set; }
    }
}
