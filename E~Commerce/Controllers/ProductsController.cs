using E_Commerce.Helper;
using Infrastructure.Specifications.ProductSpecifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using Services.Dtos.ProductsDto;
using Services.HandleResponse;
using Services.Paginations;
using Services.Services.ProductServices;
using Swashbuckle.AspNetCore.Annotations;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductServices services;

        public ProductsController(IProductServices services)
        {
            this.services = services;
        }

        [HttpGet]
        [Cach(20)]
        public async Task<ActionResult<IEnumerable<ProductDto>>>GetAllproducts([FromQuery]ProductSpecification specs)
        {
            var products = await services.GetAllProduct(specs);
            return Ok(products);
        }

        [SwaggerOperation(Summary = "{ List Of Product Using (Pagination) }")]
        [HttpGet]
        [Cach(20)]
        public async Task<ActionResult<Pagination<ProductDto>>> GetProducts([FromQuery] ProductSpecification specification)
        {
            var products = await services.GetAllProductwithpagination(specification);
            return Ok(products);
        }
        [SwaggerOperation(Summary = "{ View Product By Id } ")]
        [HttpGet]
        public async Task<ActionResult<ProductDetailsDto>>GetProductById(int ProductId)
        {
            var product=await services.GetProductById(ProductId);
            if (!product.IsSucceeded)
                return BadRequest(new ApiResponse(product.Status, product.Message));
            return Ok(product.Model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductDto dto)
        {
            var product=await services.CreateProduct(dto);
            if (!product.IsSucceeded)
                return BadRequest(new ApiResponse(product.Status,product.Message));
            return Ok(product);
        }

        [HttpPut]
        public async Task<IActionResult>UpdateProduct(int ProductId, [FromForm] UpdateProductDto dto)
        {
            var product=await services.UpdateProduct(ProductId, dto);
            if (!product.IsSucceeded)
                return BadRequest(new ApiResponse(product.Status, product.Message));
            return Ok(product.Message);
        }

        [HttpDelete]
        public async Task<IActionResult>RemoveProduct(int productId)
        {
            var product=await services.DeleteProduct(productId);
            if (!product.IsSucceeded)
                return BadRequest(new ApiResponse(product.Status, product.Message));
            return Ok(product.Message);
        }

    }
}
