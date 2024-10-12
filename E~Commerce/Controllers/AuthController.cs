using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Digests;
using Services.Dtos.AuthDtos;
using Services.Dtos.EmailDtos;
using Services.HandleResponse;
using Services.Services.AuthServices;
using System.Linq.Expressions;
using System.Net;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices authServices;

        public AuthController(IAuthServices authServices)
        {
            this.authServices = authServices;
        }


        [HttpGet]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user=await authServices.GetCurrentuser();
            return Ok(user.Model);
        }


        [HttpPost]
        public async Task<IActionResult>Register(RegisterDto dto)
        {
            var user = await authServices.Register(dto);
            if (!user.IsAuthenticated)
                return BadRequest(new ApiResponse(400,user.Message));

             SetRefreshTokenInCookie(user.RefreshToken,user.RefreshTokenExpire);
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult>Login(LoginDto dto)
        {
            var user=await authServices.Login(dto);
            if (!user.IsAuthenticated)
                return BadRequest(new ApiResponse(400,user.Message));
            if (!string.IsNullOrEmpty(user.RefreshToken))
                SetRefreshTokenInCookie(user.RefreshToken,user.RefreshTokenExpire);
            return Ok(user);

        }


        [HttpGet]
        public async Task<IActionResult> RefreshToken()
        {
            var refrehtoken = Request.Cookies["RefreshToken"];
            var result =await authServices.RefreshToken(refrehtoken);
            if (!result.IsAuthenticated)
                return BadRequest(new ApiResponse(400,result.Message));
            SetRefreshTokenInCookie(result.RefreshToken,result.RefreshTokenExpire); 
            return Ok(result);

        }

        [HttpGet]
        public async Task<IActionResult> RevokedToken([FromBody] RevokedtokenDto dto)
        {
            var Token = dto.Token ?? Request.Cookies["RefreshToken"];
            if(string.IsNullOrEmpty(Token))
                return BadRequest(new ApiResponse(400,"Token Is Rquired!"));
            var result=await authServices.RevokedOn(Token);
            if (!result)
                return BadRequest(new ApiResponse(400, "Token Is Invalid !"));
            return Ok();

        }



        [HttpPost]
        public async Task<IActionResult> ForgetPassword([FromQuery] ForgetPasswordDto dto)
        {
            var auth = await authServices.ForegetPassword(dto);
            if (!auth.IsSucceeded)
                return BadRequest(new ApiResponse(auth.Status,auth.Message));
            return Ok(new ApiResponse(auth.Status, auth.Message));
        }
        [HttpPost]
        public async Task<IActionResult> RessetPassword([FromQuery]RessetPasswordDto dto)
        {
            if (ModelState.IsValid)
            {
                var auth = await authServices.RessetPassword(dto);
                if (!auth.IsSucceeded)
                    return BadRequest(new ApiResponse(auth.Status, auth.Message));
                return Ok(auth.Message);
            }
            return BadRequest(ModelState);
        }



        [HttpPut]
        [Authorize]
        public async Task<IActionResult>EditProfile(EditProfileDto dto)
        {
            var user =await authServices.EditProfile(dto);
            if (!user.IsSucceeded)
                return BadRequest(new ApiResponse(user.Status, user.Message));
            return BadRequest(user.Message);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult>ChangePassword (ChangePasswordDto dto)
        {
            var auth=await authServices.ChangePassword(dto);
            if (!auth.IsSucceeded)
                return BadRequest(new ApiResponse(auth.Status, auth.Message));
            return Ok(auth.Message);
        }
        private void SetRefreshTokenInCookie(string refreshToken,DateTime time)
        {
            var cookie = new CookieOptions
            {
                HttpOnly = true,
                Expires = time.ToLocalTime()
            };
            Response.Cookies.Append("RefreshToken",refreshToken, cookie);
        } 
    }
}
