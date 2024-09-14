using Core.Entity.CartEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications.CartSpecifications
{
    public class CartWithCartItemSpecification : BaseSpecification<Cart>
    {
        public CartWithCartItemSpecification(CartSpecification cart) 
            : base(c=>
            (c.CartItems.Any()||cart.CartItem!=null))
        {
            AddInclude(c=> c.CartItems);
        }
    }
}
