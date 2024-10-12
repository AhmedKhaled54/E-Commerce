using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos.OrderDto;
using Services.HandleResponse;
using Services.Services.OrderServices;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderServices orderServices;

        public OrderController(IOrderServices orderServices)
        {
            this.orderServices = orderServices;
        }

        [HttpGet]
        public async Task<ActionResult<OrderResult>> GetMyOrder()
        {
            var Order = await orderServices.GetOrder();
            if (!Order.IsSucceeded)
                return BadRequest(new ApiResponse(Order.Status,Order.Message));
            return Ok(Order.Model);
        }


        [HttpPost]
        public async Task<ActionResult<OrderResult>>CreateOrder(OrderDto dto)
        {
            var order=await orderServices.CreateOrder(dto);
            if (!order.IsSucceeded)
                return BadRequest(new ApiResponse(order.Status, order.Message));
            return Ok(order.Model);
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult>CancelOrder(int OrderNumber)
        {
            var order=await orderServices.CancelOrder(OrderNumber);
            if (!order.IsSucceeded)
                return BadRequest(new ApiResponse(order.Status,order.Message));
            return Ok(order.Message);
        }
    }
}
