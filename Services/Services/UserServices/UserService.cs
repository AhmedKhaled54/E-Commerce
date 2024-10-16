using AutoMapper;
using Core.Entity;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.Dtos.AuthDtos;
using Services.Dtos.Response;
using Services.Dtos.UserDto;
using Services.Helper;
using Services.Services.UserServices.UserServices;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.UserServices
{
    public class UserService : IUserServices
    {
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOf;

        public UserService(
            UserManager<User> userManager,
            IMapper mapper,
            IUnitOfWork unitOf
            

            )
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.unitOf = unitOf;
        }
        public async Task<ResponseDto> GetAllUsers()
        {
            var user = await userManager.Users.ToListAsync();
            if (user.Count> 0)
            {
                var map=mapper.Map<List<UserDto>>(user);
                return new ResponseDto
                {
                    IsSucceeded = true,
                    Models = map,
                    Status = 200
                };
            }

            return new ResponseDto
            {
                Status = 400,
                Message = "Not Found Users!"
            };
        }

        public async Task<ResponseDto> GetUserById(string id)
        {
            var user =await userManager.Users.FirstOrDefaultAsync(u=>u.Id == id);
            if (user !=null)
            {
                var map=mapper.Map<UserDto>(user);
                return new ResponseDto
                {
                    IsSucceeded = true,
                    Model = map,
                    Status = 200
                };
            }
            return new ResponseDto
            {
                Status = 400,
                Message = "User does Not Exist!"
            };
        }

        public async Task<ResponseDto> GetUsers()
        {
            var usres = await userManager.Users.ToListAsync();
            if (usres.Count> 0)
            {
                var map=mapper.Map<List<GetUsersDto>>(usres).OrderByDescending(c=>c.UserName);
                return new ResponseDto
                {
                    IsSucceeded = true,
                    Status = 200,
                    Models = map,
                };
            }
            return new ResponseDto
            {
                Status = 400,
                Message = "Usres does Not exist!"
            };
        }

        public async Task<ResponseDto> RemoveUser(string id)
        {
            var user =await userManager.Users.FirstOrDefaultAsync(u=>u.Id==id);
            if (user != null)
            {
                await userManager.DeleteAsync(user);
                return new ResponseDto
                {
                    IsSucceeded = true,
                    Status = 200,
                    Message = "User Removed Successfuly"
                };
            }
            return new ResponseDto
            {
                Status = 400,
                Message = "Invalid User Please Try Again! "
            };
        }


        

    }
}
