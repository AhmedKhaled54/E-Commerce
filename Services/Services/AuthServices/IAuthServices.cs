using Core.Entity;
using Services.Dtos.AuthDtos;
using Services.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.AuthServices
{
    public  interface IAuthServices
    {
        Task<AuthModel>Register(RegisterDto dto);
        Task<AuthModel> Login(LoginDto dto);
        Task<AuthModel> RefreshToken(string token);
        Task<bool> RevokedOn(string token);

        Task<User> GetCurrentuser();
    }
}
