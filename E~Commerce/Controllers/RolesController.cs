using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.HandleResponse;
using Services.Services.RoleServices;

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

        [HttpPost]
        public async Task<IActionResult>AddNewRole([FromBody]string RoleName)
        {
            var role=await roleServices.AddNewRole(RoleName);
            if (!role.IsSucceeded)
                return BadRequest(new ApiResponse(role.Status,role.Message));

           return Ok(new ApiResponse(role.Status,role.Message));
        }
    }
}
