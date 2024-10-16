using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos.UserDto;
using Services.HandleResponse;
using Services.Services.UserServices.UserServices;
using Swashbuckle.AspNetCore.Annotations;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    
    public class UsersController : ControllerBase
    {
        private readonly IUserServices userServices;

        public UsersController(IUserServices userServices)
        {
            this.userServices = userServices;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            var users = await userServices.GetAllUsers();
            if (!users.IsSucceeded)
                return BadRequest(new ApiResponse(users.Status, users.Message));
            return Ok(users.Models);
        }
        [HttpGet]
        [SwaggerOperation(Summary ="{Get Usere Using Drop down List}")]
        public async Task<ActionResult<IEnumerable<GetUsersDto>>> GetUsers()
        {
            var users =await userServices.GetUsers();
            if (!users.IsSucceeded)
                return BadRequest (new ApiResponse(users.Status, users.Message));
            return Ok(users.Models);
        }

       [HttpGet]
        public async Task<ActionResult<UserDto>>GetUserById(string UserId)
        {
            var user=await userServices.GetUserById(UserId);
            if (!user.IsSucceeded)
                return BadRequest(new ApiResponse(user.Status, user.Message));
            return Ok(user.Model);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveUser(string UserId)
        {
            var user = await userServices.RemoveUser(UserId);
            if (!user.IsSucceeded)
                return BadRequest(new ApiResponse(user.Status, user.Message));
            return Ok(new ApiResponse(user.Status, user.Message));
        }
    }
}
