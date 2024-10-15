using AutoMapper;
using Core.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.Dtos.Response;
using Services.Dtos.RoleDtos;
using Services.HandleResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.RoleServices
{
    public class RoleServices : IRoleServices
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;

        public RoleServices(
            RoleManager<IdentityRole>  roleManager,
            UserManager<User> userManager,
            IMapper mapper 
            )
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.mapper = mapper;
        }


        public async Task<ResponseDto> GetAllRoles()
        {
            var role =await roleManager.Roles.ToListAsync();
            if (role.Any())
            {
                var MapRole = mapper.Map<List<RoleDetailsDto>>(role);
                return new ResponseDto
                {
                    
                    IsSucceeded=true,
                    Status=200,
                    Models=MapRole
                };
            }

            return new ResponseDto
            {
                Status = 400,
                Models = null,
                Message = "An error occured or there is no Roles!"
            };
        }
        public async Task<ResponseDto> AddNewRole(AddOrUpdateRoleDto dto)
        {
            if (await roleManager.RoleExistsAsync(dto.RoleName))
                return new ResponseDto { Status = 400, Message = "Role Name Aleready Saved !" };
            var role = new IdentityRole();
            role.Name = dto.RoleName;   
            await roleManager.CreateAsync(role);
            return new ResponseDto
            {
                Status = 200,
                Message = $"Role {dto.RoleName} Saved Successfuly"
            };
            
        }

        public async Task<ResponseDto> AddRoleToUser(AddRoleToUserDto dto)
        {
            var response= new ResponseDto();
            var user =await userManager.FindByIdAsync(dto.UserId);
            if (user is null||!await roleManager.RoleExistsAsync(dto.RoleName))
            {
                response.Message = "Invalid User OR Role  Please Try Again !";
                response.Status= 400;
                return response;
            }

            if (await userManager.IsInRoleAsync(user, dto.RoleName))
            {
                response.Message = "User Already Asseign  To Role!";
                response.Status = 400;
                return response;
            }

              await userManager.AddToRoleAsync(user,dto.RoleName);
                response.IsSucceeded = true;
                response.Status = 200;
                response.Message=$"Successfuly Add User In This Role {dto.RoleName}";
                return response;
           
        }

       
        public async Task<ResponseDto> UpdateRole(string roleId, AddOrUpdateRoleDto dto)
        {
            var response=new ResponseDto();
            var role=await roleManager.Roles.FirstOrDefaultAsync(r=>r.Id==roleId);
            if (role is null)
            {
                response.Status = 400;
                response.Message = "Invalid Role Please Try Again!";
                return response;
            }
            role.Name=dto.RoleName;
            await roleManager.UpdateAsync(role);
            response.IsSucceeded = true;
            response.Status = 200;
            response.Message = "Role Updated Successfuly";
            return response;
            

        }

        public async  Task<ResponseDto> RemoveRole(string roleId)
        {
            var response=new ResponseDto();

            var role =await roleManager.Roles.FirstOrDefaultAsync(r=>r.Id==roleId);
            if (role is null)
            {
                response.Status = 400;
                response.Message = "Invalid Role Please Try Again!";
                return response;
            }
            await roleManager.DeleteAsync(role);
            response.IsSucceeded=true;
            response.Message = "Role Deleted Successfuly";
            response.Status = 200;

            return response;
            




        }

    }
}
