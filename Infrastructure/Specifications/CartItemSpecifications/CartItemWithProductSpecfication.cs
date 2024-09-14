using Core.Entity.CartEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications.CartItemSpecifications
{
    public class CartItemWithProductSpecfication : BaseSpecification<CartItem>
    {
        public CartItemWithProductSpecfication(CartItemSpecification cartItem) :
            base(c=>
            (!cartItem.ProductId.HasValue||c.productId==cartItem.ProductId)
            )
        {
            AddInclude(p => p.Product);
        }
    }
}
