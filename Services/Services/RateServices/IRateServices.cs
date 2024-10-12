using Services.Dtos.RateDtos;
using Services.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.RateServices
{
    public  interface IRateServices
    {
        Task<ResponseDto> AddRate(AddRateDto dto);
        Task<double> GetavarageForProduct(int ProductId);
        Task<ResponseDto> GetProductsRate(int ProductId );
    }
}
