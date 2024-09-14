using Services.Dtos.BrandsDto;
using Services.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.BrandServices
{
    public interface IBrandServices
    {
        Task<ResponseDto>GetAll();
        Task<ResponseDto>GetById(int id);
        Task<ResponseDto>Create(CreateOrUpdateBrandDto dto);
        Task<ResponseDto> Update(int id,CreateOrUpdateBrandDto dto);
        Task<ResponseDto> DeleteById(int id);
    }
}
