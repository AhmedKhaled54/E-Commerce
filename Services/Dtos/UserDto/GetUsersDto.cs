using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.UserDto
{
    public  class GetUsersDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
