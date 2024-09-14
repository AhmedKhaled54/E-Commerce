using Core.Entity.CartEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications.CartSpecifications
{
    public class CartSpecification
    {
        public CartItem ?CartItem { get; set; }
    }
}
