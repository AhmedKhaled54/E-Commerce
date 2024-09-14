using Services.Dtos.CatgegoryDto;
using Services.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.CategoryServices
{
    public interface ICategoryServices
    {
        Task<ResponseDto> GetAll();
        Task<ResponseDto> GetByid(int id);
        Task<ResponseDto> Create(CreateOrUpdateCategory dto);
        Task<ResponseDto> Update(int id, CreateOrUpdateCategory dto);
        Task<ResponseDto> Delete(int id);
    }
}
