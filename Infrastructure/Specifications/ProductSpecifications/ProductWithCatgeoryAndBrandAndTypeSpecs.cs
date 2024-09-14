using Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications.ProductSpecifications
{
    public class ProductWithCatgeoryAndBrandAndTypeSpecs : BaseSpecification<Product>
    {
        public ProductWithCatgeoryAndBrandAndTypeSpecs(ProductSpecification product) 
            : base
            (c=>
            (!product.Category.HasValue||c.CategoryId==product.Category)&&
            (!product.Brand.HasValue||c.Productbrand_Id==product.Brand)&&
            (!product.Type.HasValue||c.productType_Id==product.Type)&&
            (string.IsNullOrEmpty(product.Search)||c.Name.Trim().ToLower().Contains(product.Search))
            )
        {  
            AddInclude(c=>c.Category);
            AddInclude(t=>t.ProductType);
            AddInclude(b=> b.ProductBrand);
            AddOrderBy(o=>o.Name);

            if (!string.IsNullOrEmpty(product.Sort))
            {
                switch (product.Sort)
                {
                    case "PriceAsc"
                    :AddOrderBy(o => o.Price);
                        break;
                    case "PriceDesc"
                    :AddOrderByDesc(o=>o.Price);
                        break;
                    default
                    :AddOrderBy(o => o.Name);
                        break;
                }
            }

            AddPagination(product.PageSize*(product.PageIndex-1),product.PageSize);


          
        }



        public ProductWithCatgeoryAndBrandAndTypeSpecs(int? productId):base (c=>c.Id==productId)
        {
            AddInclude(c => c.Category);
            AddInclude(t => t.ProductType);
            AddInclude(b => b.ProductBrand);
        }
    }
}
