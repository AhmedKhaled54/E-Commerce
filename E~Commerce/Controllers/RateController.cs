using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos.RateDtos;
using Services.HandleResponse;
using Services.Services.RateServices;
using Swashbuckle.AspNetCore.Annotations;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RateController : ControllerBase
    {
        private readonly IRateServices rateServices;

        public RateController(IRateServices rateServices)
        {
            this.rateServices = rateServices;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "{Get All Rate For Products }")]

        public async Task<IActionResult>GetProductsRates(int ProductId)
        {
            var Rate =await rateServices.GetProductsRate(ProductId);
            if (!Rate.IsSucceeded)
                return BadRequest(new ApiResponse(Rate.Status,Rate.Message));
            return Ok(Rate.Model);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "{ Get Avarage For Product }")]

        public async Task<IActionResult>GetAvarageforProduct(int ProductId)
        {
            var rate =await rateServices.GetavarageForProduct(ProductId);
            return Ok(rate);
        }
        [HttpPost]
        [SwaggerOperation(Summary = "{ Add User Rate For Product   }")]

        public async Task<IActionResult>AddUserRate(AddRateDto dto)
        {
            var rate = await rateServices.AddRate(dto);
            if (!rate.IsSucceeded)
                 return BadRequest(new ApiResponse(rate.Status,rate.Message));

            return Ok(new ApiResponse(rate.Status,rate.Message));
        }

        
    }
}
