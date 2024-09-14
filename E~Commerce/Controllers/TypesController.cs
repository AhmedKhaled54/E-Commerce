using AutoMapper.Configuration.Conventions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos.TypeDto;
using Services.HandleResponse;
using Services.Services.ProductTypeServices;
using Swashbuckle.AspNetCore.Annotations;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TypesController : ControllerBase
    {
        private readonly ITypeServices services;

        public TypesController(ITypeServices services)
        {
            this.services = services;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "{ List Of Product Type  Using Drop down List }")]
        public async Task<ActionResult<IEnumerable<TypeDto>>> GetAllTypes()
        {
            var type = await services.GetAll();
            if (!type.IsSucceeded)
                return BadRequest(new ApiResponse(type.Status,type.Message));
            return Ok(type.Models);
        }

        [HttpGet]
        public async Task<ActionResult<TypeDto>>GetTypeById(int TypeId)
        {
            var type = await services.GetById(TypeId);
            if (!type.IsSucceeded) 
                return BadRequest(new ApiResponse(type.Status, type.Message));
            return Ok(type.Model);
        }
        [HttpPut]
        public async Task<IActionResult>UpdateType(int TypeId,CreateOrUpdateTypeDto dto)
        {
            var type=await services.Update(TypeId, dto);    
            if (!type.IsSucceeded)
                return BadRequest(new ApiResponse(type.Status, type.Message));
            return Ok(type.Message);
        }

        [HttpDelete]
        public async Task<IActionResult>RemoveType(int TypeId)
        {
            var type=await services.DeleteById(TypeId);
            if (!type.IsSucceeded)
                return BadRequest(new ApiResponse(type.Status, type.Message));
            return Ok(type.Message);
        }
    }
}
