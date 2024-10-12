using Core.Entity;
using Services.Dtos.AuthDtos;
using Services.Dtos.Response;
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
        Task<ResponseDto> GetCurrentuser();
        //
        Task<ResponseDto> ForegetPassword(ForgetPasswordDto dto );
        Task<ResponseDto> RessetPassword(RessetPasswordDto dto);
        Task<ResponseDto> EditProfile(EditProfileDto dto);  

        Task<ResponseDto>ChangePassword(ChangePasswordDto dto);


    }
}
