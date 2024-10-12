using AutoMapper;
using Core.Entity;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Dtos.AuthDtos;
using Services.Dtos.EmailDtos;
using Services.Dtos.Response;
using Services.Dtos.UserDto;
using Services.Dtos.WishListDto;
using Services.EmailServices;
using Services.HandleResponse;
using Services.Helper;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.AuthServices
{
    public class AuthServices : IAuthServices
    {
        private readonly UserManager<User> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IEmailServices emailServices;
        private readonly IUnitOfWork _unitOf;
        private readonly IMapper mapper;
        private readonly JWT JWT;
        private static ConcurrentDictionary<string,string> OtpStorage = new ConcurrentDictionary<string,string>();
        public AuthServices(UserManager<User> userManager,
            IOptions<JWT>_JWt,
            IHttpContextAccessor httpContextAccessor,
            IEmailServices emailServices,
            IUnitOfWork unitOf,
            IMapper mapper
            )
        {
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
            this.emailServices = emailServices;
            _unitOf = unitOf;
            this.mapper = mapper;
            JWT =_JWt.Value;
        }


        public async Task<AuthModel> Register(RegisterDto dto)
        {
            if (await userManager.FindByEmailAsync(dto.Email)!=null)
                    return new AuthModel { Message = "Email Already Register!" };
            if (await userManager.FindByNameAsync(dto.UserName) != null)
                return new AuthModel { Message = "UserName Already Register!" };

            var user = new User
            {
                UserName = dto.UserName,
                Email = dto.Email,
                Address = dto.Address,
                TwoFactorEnabled=true
            };

            var result =await userManager.CreateAsync(user,dto.Password);
            if (!result.Succeeded)
            {
                var message = string.Empty;
               foreach(var errors in result.Errors)
               {
                    message+= $"{errors.Description},";
               }
                return new AuthModel { Message = message };
            }

            var role = await userManager.AddToRoleAsync(user, "User");

            var Token = await Createtoken(user);
            var RefreshToken = GenerateRefreshToken();
            user.RefreshTokens?.Add(RefreshToken);
            await userManager.UpdateAsync(user);
            return new AuthModel
            {
                UserName = user.UserName,
                Email = user.Email,
                Token = new JwtSecurityTokenHandler().WriteToken(Token),
                IsAuthenticated = true,
                Roles=new List<string> { "User"},
                RefreshToken=RefreshToken.Token,
                RefreshTokenExpire=RefreshToken.ExpireOn,
                Message=$"Welcome {user.UserName}"
            };



        }
        public async Task<AuthModel> Login(LoginDto dto)
        {
            var authmodel = new AuthModel();

            var user=await userManager.FindByEmailAsync(dto.Email);
            if (!await userManager.CheckPasswordAsync(user, dto.Password) || user == null)
            {
                authmodel.Message = "Incorrect Email Or Password!";
                return authmodel;
            }

            var roles=await userManager.GetRolesAsync(user);
            var Token=await Createtoken(user);
            authmodel.Token=new JwtSecurityTokenHandler().WriteToken(Token);
            authmodel.Email=dto.Email;
            authmodel.UserName=user.UserName;
            authmodel.Roles=roles.ToList();
            authmodel.Message = $"Welcome {user.UserName}";
            authmodel.IsAuthenticated = true;
            if (user.RefreshTokens.Any(r => r.IsActive))
            {
                var ActiveToken = user.RefreshTokens.Single(c => c.IsActive);
                authmodel.RefreshToken=ActiveToken.Token;
                authmodel.RefreshTokenExpire=ActiveToken.ExpireOn;
            }
            else
            {
                var RefreshToken = GenerateRefreshToken();
                authmodel.RefreshToken=RefreshToken.Token;
                authmodel.RefreshTokenExpire=RefreshToken.ExpireOn;
                user.RefreshTokens.Add(RefreshToken);
                await userManager.UpdateAsync(user);
            }

            return authmodel;
        }

       

        public async Task<AuthModel> RefreshToken(string token)
        {
            var authmodel=new AuthModel();
            var user = await userManager.Users.SingleOrDefaultAsync(c => c.RefreshTokens.Any(r => r.Token == token));
            if (user == null)
            {
                authmodel.Message = "Invalid Token!";
                return authmodel;
            }
            var RefreshToken=user.RefreshTokens.Single(c=>c.Token == token);
            if (!RefreshToken.IsActive)
            {
                authmodel.Message = "InActive Token!";
                return authmodel;
            }
            
            RefreshToken.RevokedOn = DateTime.UtcNow;
            var NewRefreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(NewRefreshToken);
            await userManager.UpdateAsync(user);
            var Jwt =await  Createtoken(user);
            var roles=await userManager.GetRolesAsync(user);
            authmodel.Email=user.Email;
            authmodel.UserName=user.UserName;
            authmodel.Roles=roles.ToList();
            authmodel.IsAuthenticated=true;
            authmodel.RefreshToken=NewRefreshToken.Token;
            authmodel.RefreshTokenExpire=NewRefreshToken.ExpireOn;

            return authmodel;
        }
        public async Task<bool> RevokedOn(string token)
        {
            var user = await userManager.Users.SingleOrDefaultAsync(c => c.RefreshTokens.Any(r => r.Token == token));
            if (user == null)
                return false;

            var refreshtoken=user.RefreshTokens.Single(c=>c.Token == token);
            if (!refreshtoken.IsActive)
                return false;
           refreshtoken.RevokedOn = DateTime.UtcNow;    
            await userManager.UpdateAsync(user);
            return true;

        }


        public async Task<ResponseDto> GetCurrentuser()
        {
            var email =httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);
           //var user = await userManager.Users.FirstAsync(c => c.Email == email);

            var User =await userManager.Users
                .Include(c=>c.wishLists).FirstOrDefaultAsync(e=>e.Email == email);
            var wishlist =await  _unitOf.Repository<WishList>().GetAllPredicated(c => c.UserId == User.Id, new[] { "Product" });
            var MapWishList=mapper.Map<List<WishListsDto>>(wishlist);
            var MapUser=mapper.Map<UserProfileDto>(User);
            return new ResponseDto
            {
                IsSucceeded = true,
                Model=MapUser,
            };

        }
        private async Task<JwtSecurityToken>Createtoken(User user)
        {
           var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWT.Key));
            var signin = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(

                issuer: JWT.Issuer,
                audience: JWT.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(JWT.DurationInDayes),
                signingCredentials: signin);
            

           /* var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name,user.UserName));
            claims.Add(new Claim(ClaimTypes.NameIdentifier,user.Id));
            claims.Add(new Claim(ClaimTypes.Email,user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid ().ToString()));
            var Roles = await userManager.GetRolesAsync(user);
            foreach(var ItemRole in Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, ItemRole));
            }

            var key =new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWT.Key));
            var SigningCredentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(
                issuer: JWT.Issuer,
                audience: JWT.Audience,
                claims: claims,
                signingCredentials: SigningCredentials,
                expires: DateTime.Now.AddDays(JWT.DurationInDayes)
                );*/
            return token; 

        }

        private RefreshToken GenerateRefreshToken()
        {
            var rendomNumber =new byte[32];
            using var generator = new RNGCryptoServiceProvider();
            generator.GetBytes(rendomNumber);
            return new RefreshToken
            {
                ExpireOn = DateTime.UtcNow.AddDays(7),
                Token = Convert.ToBase64String(rendomNumber),
                CreatedOn = DateTime.UtcNow

            };



        }

        public async Task<ResponseDto> ForegetPassword(ForgetPasswordDto dto)
        {
            var user =await userManager.FindByEmailAsync(dto.Email);
            if (user != null)
            {
                var token =await userManager.GeneratePasswordResetTokenAsync(user);
                var otpCode = GenerateOtpCode(6);



                var email = new EmailDto
                {
                    ToEmail = dto.Email,
                    Subject = "ForgetPassword",
                    Body =$" IS Your OTP  Code:\a{otpCode}\a.Do Not Share it With anyone"
                };
                await emailServices.SendEmail(email);
                OtpStorage[dto.Email]=otpCode;

                return new ResponseDto
                {
                    Status = 200,
                    IsSucceeded = true,
                    Message = $"Password Change Request is send email <{user.Email}> ..please open your Email"
                };
            }

            return new ResponseDto
            {
                Status = 400,
                Message = " couldn't send email ,please try Again !"
            };
        }

        public async  Task<ResponseDto> RessetPassword(RessetPasswordDto dto)
        {
            var response = new ResponseDto();
            if (!OtpStorage.TryGetValue(dto.Email,out var otpstorage) || otpstorage != dto.Code)
            {
                response.Status = 400;
                response.Message = "Invalid OtpCode Please Try Again!";
                return response;
            }

            var user = await userManager.FindByEmailAsync(dto.Email);
            if (user != null)
            {
                //hashPassword 
                var hashPassword =userManager.PasswordHasher.HashPassword(user,dto.NewPassword);
                user.PasswordHash = hashPassword;
                await userManager.ChangePasswordAsync(user,user.PasswordHash,dto.NewPassword);
                await userManager.UpdateAsync(user);
                OtpStorage.TryRemove(dto.Email, out _);
                response.IsSucceeded = true;
                response.Status = 200;
                response.Message = "Password has been changed successfully";
                return response;
            }
            response.Status = 400;
            response.Message = "Invalid User!";
            return response;
        }

        public async Task<ResponseDto> EditProfile(EditProfileDto dto)
        {
            
            var user = await GetUser();
            if (user != null)
            {
                user.Email = dto.Email;
                user.UserName=dto.Name;
                user.Address = dto.Address;
                await userManager.UpdateAsync(user);
                return new ResponseDto
                {
                    IsSucceeded = true,
                    Status = 200,
                    Message = "Updated Data Successfuly"
                };
            }
            return new ResponseDto
            {
                Status = 400,
                Message = "Invalid User Please Try Again!"
            };
        }

        public async Task<ResponseDto> ChangePassword(ChangePasswordDto dto)
        {
            var response = new ResponseDto();
            var user= await GetUser();
            if (user==null )
            {
                response.Message = "Invalid User";
                response.Status = 400;
                return response;

            }
            var result = await userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            if (!result.Succeeded)
            {
                 var error=string.Empty;
                foreach(var Errors in result.Errors)
                {
                    error += $"{Errors.Description},";
                }
                response.Message = error;
                response.Status = 400;
                return response;
            }
            response.IsSucceeded = true;
            response.Message = "Password Changed Succesfuly";
            response.Status = 200;
            return response;
        }
        private string GenerateOtpCode(int lenth)
        {
            var random = new Random();
            string Otp = string.Empty;
            for (int r=0;r<lenth;r++)
            {
                Otp += random.Next(0, 10).ToString();
                
            }
            return Otp;
        }

        
        private async Task<User>GetUser()
        {
            ClaimsPrincipal  User = httpContextAccessor.HttpContext.User;
            var userid=userManager.GetUserId(User);
            var users= await userManager.Users.FirstOrDefaultAsync(x => x.Id==userid);
            return users;
        }

        
    }
}
