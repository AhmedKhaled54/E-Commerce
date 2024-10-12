using Core.Migrations;
using Infrastructure.Specifications.WIshListsSpecification;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos.WishListDto;
using Services.HandleResponse;
using Services.Paginations;
using Services.Services.WishListServices;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WishListsController : ControllerBase
    {
        private readonly IWishListServices services;

        public WishListsController(IWishListServices services)
        {
            this.services = services;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WishListsDto>>> GetWishList()
        {
            var wishlist=await services.GetWishList();
            if (!wishlist.IsSucceeded)
                return BadRequest(new ApiResponse(wishlist.Status, wishlist.Message));
            return Ok(wishlist.Models);
        }


        [HttpPost]
        public async Task<IActionResult>AddToWishlist([FromQuery]AddORDeleteWIshListDto dto)
        {
            var wishlist=await services.AddWishList(dto);
            if (!wishlist.IsSucceeded)
                return BadRequest(new ApiResponse(wishlist.Status,wishlist.Message));
            return Ok(wishlist.Message);
        }


        [HttpDelete]

        public async Task<IActionResult>DeleteWIshList([FromQuery]AddORDeleteWIshListDto dto)
        {
            var wishlist=await services.RemoveWIshList(dto);
            if (!wishlist.IsSucceeded)
                return BadRequest(new ApiResponse(wishlist.Status, wishlist.Message));
            return Ok(wishlist.Message);
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<WishListsDto>>> Get([FromQuery]WishListSpecification specification )
        {
            var wish = await services.GetPagination(specification);
            return Ok(wish);
        }
    }
}
