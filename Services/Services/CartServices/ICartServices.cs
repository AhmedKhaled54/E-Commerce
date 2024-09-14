using Core.Entity.CartEntity;
using Services.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.CartServices
{
    public  interface ICartServices
    {
        Task<ResponseDto> GetCart();
        Task<Cart> GetCartforuser(string userid);
        Task<ResponseDto> AddCart(int productid, int quantity);
        Task<ResponseDto>RemoveproductfromCart(int productid,int quantity);
        Task RemoveItemBecomeOrder(string userid);
    }
}
