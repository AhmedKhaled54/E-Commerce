using Services.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.RoleServices
{
    public interface IRoleServices
    {
        Task<ResponseDto> AddNewRole(string RoleName);
    }
}
