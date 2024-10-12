using Services.Dtos.OrderDto;
using Services.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.OrderServices
{
    public  interface IOrderServices
    {
        Task<ResponseDto>CreateOrder(OrderDto orderDto);
        Task<ResponseDto>GetOrder();
        Task<ResponseDto> CancelOrder(int OrderNumber);
    }
}
