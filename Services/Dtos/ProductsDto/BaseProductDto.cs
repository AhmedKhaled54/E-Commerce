using Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.ProductsDto
{
    public  class BaseProductDto
    {
        [Required(ErrorMessage ="Please Enter Product Name !")]
        public string ProductName { get; set; }
        [Required]
        public string Description { get; set; }
        [Required(ErrorMessage ="Please Enter Product Price !")]
        public decimal Price { get; set; }

        //public int Brand {  get; set; }
        //public int Category { get; set; }
        //public int Type { get; set; }       

        public int CategoryId { get; set; }
       
        public int Productbrand_Id { get; set; }

        public int productType_Id { get; set; }


    }
}
