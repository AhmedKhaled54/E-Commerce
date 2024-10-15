using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Dtos.RoleDtos;
using Services.HandleResponse;
using Services.Services.RoleServices;
using Swashbuckle.AspNetCore.Annotations;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleServices roleServices;

        public RolesController(IRoleServices roleServices)
        {
            this.roleServices = roleServices;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleDetailsDto>>> GetAllRoles()
        {
            var roles = await roleServices.GetAllRoles();
            if (!roles.IsSucceeded)
                return BadRequest(new ApiResponse(roles.Status, roles.Message));
            return Ok(roles.Models);
        }

        [HttpPost]
        public async Task<IActionResult>AddNewRole(AddOrUpdateRoleDto dto)
        {
            var role=await roleServices.AddNewRole(dto);
            if (!role.IsSucceeded)
                return BadRequest(new ApiResponse(role.Status,role.Message));

           return Ok(new ApiResponse(role.Status,role.Message));
        }

        [HttpPost]
        [SwaggerOperation(Summary = "{ Add User To Role }")]
        public async Task<IActionResult>AsseignUserToRole (AddRoleToUserDto dto)
        {
            var role = await roleServices.AddRoleToUser(dto);
            if (!role.IsSucceeded)
                return BadRequest(new ApiResponse(400, role.Message));

            return Ok(new ApiResponse(role.Status,role.Message));
        }
        [HttpPut]
        public async Task<IActionResult>UpdateRole(string RoleId ,AddOrUpdateRoleDto dto)
        {
            var role =await roleServices.UpdateRole(RoleId,dto);
            if (!role.IsSucceeded)
                return BadRequest (new ApiResponse(role.Status,role.Message));

            return Ok(new ApiResponse(role.Status,role.Message));

        }


        [HttpDelete]
        public async Task<IActionResult>RemoveRole(string RoleId)
        {
            var role = await roleServices.RemoveRole(RoleId);
            if (!role.IsSucceeded)
                return BadRequest(new ApiResponse(role.Status, role.Message));

            return Ok(new ApiResponse(role.Status, role.Message));

        }

    }
}
