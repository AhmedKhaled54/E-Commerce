using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos.CartsDto;
using Services.HandleResponse;
using Services.Services.CartServices;


namespace E_Commerce.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartServices services;
        

        public CartController(ICartServices  services)
        {
            this.services = services;
            
        }

        [HttpGet]
        public async Task<ActionResult<CartDto>> GetCartForUser()
        {
            var carts = await services.GetCart();
            if (!carts.IsSucceeded)
                return BadRequest(new ApiResponse(carts.Status, carts.Message));
            return Ok(carts.Model);

        }

        [HttpPost]
        public async Task<IActionResult>AddProductToCart(int ProductId ,int quantity)
        {
            var cart=await services.AddCart(ProductId,quantity);
            if (!cart.IsSucceeded)
                return BadRequest(new ApiResponse(cart.Status, cart.Message));
            return Ok(cart.Message);
        }

        [HttpDelete]
        public async Task<IActionResult>RemoveProductFromCart(int ProductId ,int quantity)
        {
            var cart=await services.RemoveproductfromCart(ProductId,quantity);
            if (!cart.IsSucceeded)
                return BadRequest(new ApiResponse(cart.Status,cart.Message));   
            return Ok(cart.Message);
        }

    }
}
