using Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications.WIshListsSpecification
{
    public class WishListWithProductSecification : BaseSpecification<WishList>
    {
        public WishListWithProductSecification(WishListSpecification wishList) : base(x=>(x.Product!=null))
        {
            AddPagination(wishList.PageSize * (wishList.PageIndex - 1), wishList.PageSize);

        }
    }
}
