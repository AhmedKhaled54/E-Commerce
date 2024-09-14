using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos.AuthDtos;
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
            return Ok(user);
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
