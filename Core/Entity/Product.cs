using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity
{
    public  class Product:BaseEntity 
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string? Image { get; set; }
        [ForeignKey(nameof(Category))]
        public int CategoryId  { get; set; }   
        [ForeignKey(nameof(ProductBrand))]
        public int Productbrand_Id { get; set; }
        [ForeignKey(nameof(ProductType))]
        public int productType_Id { get; set; }
        public virtual ProductBrand ProductBrand { get; set; }  
        public virtual ProductType  ProductType { get; set; }   
        public virtual Category Category { get; set; }      
        public List<Rate>? Rate { get; set; }
    }
}
