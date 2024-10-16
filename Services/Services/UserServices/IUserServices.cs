using Services.Dtos.AuthDtos;
using Services.Dtos.Response;
using Services.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.UserServices.UserServices
{
    public  interface IUserServices
    {
        Task<ResponseDto> GetAllUsers();
        Task<ResponseDto> GetUsers();//DropDownList
        Task<ResponseDto>GetUserById(string id);
        Task<ResponseDto> RemoveUser(string id);

    }
}
