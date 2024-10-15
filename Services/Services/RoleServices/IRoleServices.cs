using Services.Dtos.Response;
using Services.Dtos.RoleDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.RoleServices
{
    public interface IRoleServices
    {
        Task<ResponseDto> GetAllRoles();
        Task<ResponseDto> AddNewRole(AddOrUpdateRoleDto dto);
        Task<ResponseDto> AddRoleToUser(AddRoleToUserDto dto);
        Task<ResponseDto> UpdateRole(string roleId, AddOrUpdateRoleDto dto);
        Task<ResponseDto> RemoveRole(string roleId);
    }
}
