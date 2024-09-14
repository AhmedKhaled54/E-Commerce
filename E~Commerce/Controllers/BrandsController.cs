using AutoMapper.Configuration.Conventions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos.BrandsDto;
using Services.HandleResponse;
using Services.Services.BrandServices;
using Swashbuckle.AspNetCore.Annotations;
using System.Runtime.InteropServices;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandServices services;

        public BrandsController(IBrandServices services)
        {
            this.services = services;
        }

        [SwaggerOperation(Summary = "{ List Of Product Brand Using Drop down List }")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BrandDto>>> GetAllBrand()
        {
            var brand = await services.GetAll();
            if (!brand.IsSucceeded)
                return BadRequest(new ApiResponse(brand.Status,brand.Message));
            return Ok(brand.Models);
        }

        [HttpGet]
        public async Task<ActionResult<BrandDto>>GetBrandById(int BrandId)
        {
            var brand=await services.GetById(BrandId);
            if (!brand.IsSucceeded)
                return BadRequest(new ApiResponse(brand.Status,brand.Message));
            return Ok(brand.Model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBrand(CreateOrUpdateBrandDto dto)
        {
            if (ModelState.IsValid)
            {
                var brand = await services.Create(dto);
                if (!brand.IsSucceeded)
                    return BadRequest(new ApiResponse(brand.Status, brand.Message));
                return Ok(brand.Model);
            }
            return BadRequest(ModelState);
        }
        

        [HttpPut]
        public async Task<IActionResult>UpdateBrand(int BrandId ,CreateOrUpdateBrandDto dto)
        {
            var brand =await services.Update(BrandId, dto);
            if (!brand.IsSucceeded)
                return BadRequest(new ApiResponse(brand.Status, brand.Message));
            return Ok(brand.Message);
        }

        [HttpDelete]
        public async Task<IActionResult>RemoveBrand(int BrandId)
        {
            var brand=await services .DeleteById(BrandId);
            if (!brand.IsSucceeded)
                return BadRequest(new ApiResponse(brand.Status, brand.Message));
            return Ok(brand.Message);
        }
    }
}
