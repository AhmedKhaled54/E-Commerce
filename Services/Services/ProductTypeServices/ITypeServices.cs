using Services.Dtos.Response;
using Services.Dtos.TypeDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.ProductTypeServices
{
    public  interface ITypeServices
    {
        Task<ResponseDto> GetAll();
        Task<ResponseDto> GetById(int id);
        Task<ResponseDto> Create(CreateOrUpdateTypeDto dto);
        Task<ResponseDto> Update(int id ,CreateOrUpdateTypeDto dto);
        Task<ResponseDto> DeleteById(int id);
    }
}
