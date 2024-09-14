using Core.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.Dtos.Response;
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

        public RoleServices(RoleManager<IdentityRole>  roleManager,UserManager<User> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        public async Task<ResponseDto> AddNewRole(string RoleName)
        {
            if (await roleManager.RoleExistsAsync(RoleName))
                return new ResponseDto { Status = 400, Message = "Role Name Aleready Saved !" };
            var role = new IdentityRole();
            role.Name = RoleName;   
            await roleManager.CreateAsync(role);
            return new ResponseDto
            {
                Status = 200,
                Message = $"Role {RoleName} Saved Succesfuly"
            };
            
        }
    }
}
