using Infrastructure.Specifications.ProductSpecifications;
using Services.Dtos.ProductsDto;
using Services.Dtos.Response;
using Services.Paginations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.ProductServices
{
    public  interface IProductServices
    {
        Task<IEnumerable<ProductDto>> GetAllProduct(ProductSpecification  specification);
        Task<ResponseDto>GetProductById (int? id );
        Task<Pagination<ProductDto>>GetAllProductwithpagination(ProductSpecification specification);
        Task<ResponseDto> CreateProduct(CreateProductDto dto);
        Task<ResponseDto> UpdateProduct(int id,UpdateProductDto dto);
        Task<ResponseDto> DeleteProduct(int id);

    }
}
