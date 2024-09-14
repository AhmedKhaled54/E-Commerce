using Core.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos.CatgegoryDto;
using Services.HandleResponse;
using Services.Services.CategoryServices;
using Swashbuckle.AspNetCore.Annotations;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryServices services;

        public CategoryController(ICategoryServices services)
        {
            this.services = services;
        }

        [SwaggerOperation(Summary = "{ List Of  Category  Using Drop down List }")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDetailsDto>>> GetAllCategory()
        {
            var category = await services.GetAll();
            if (!category.IsSucceeded)
                return BadRequest(new ApiResponse(category.Status, category.Message));
            return Ok(category.Models);
        }


        [HttpGet]
        public async Task<ActionResult<CategoryDetailsDto>>GetCatgeoryById(int categoryId)
        {
            var category=await services.GetByid(categoryId);   
            if (!category.IsSucceeded)
                return BadRequest(new ApiResponse(category.Status,category.Message));
            return Ok(category.Model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateOrUpdateCategory dto)
        {
            var category=await services.Create(dto);    
            if (!category.IsSucceeded)
                return BadRequest(new ApiResponse(category.Status, category.Message));
            return Ok(category);

        }
        [HttpPut]
        public async Task<IActionResult>UpdateCategory(int CategoryId,CreateOrUpdateCategory dto)
        {
            var category=await services.Update(CategoryId,dto);
            if (!category.IsSucceeded)
                return BadRequest(new ApiResponse(category.Status, category.Message));
            return Ok(category.Message);
        }

        [HttpDelete]
        public async Task<IActionResult>RemoveCategory(int CategoryId)
        {
            var category=await services.Delete(CategoryId);
            if (!category.IsSucceeded)
                return BadRequest(new ApiResponse(category.Status, category.Message));
            return Ok(category.Message);
        }
    }
}
