using Infrastructure.Specifications.WIshListsSpecification;
using Services.Dtos.Response;
using Services.Dtos.WishListDto;
using Services.Paginations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.WishListServices
{
    public  interface IWishListServices
    {
        Task<ResponseDto> GetWishList();
        Task<ResponseDto> AddWishList(AddORDeleteWIshListDto dto);
        Task<ResponseDto> RemoveWIshList(AddORDeleteWIshListDto dto);
        
        Task<Pagination<WishListsDto>>GetPagination(WishListSpecification specification);
    }
}
