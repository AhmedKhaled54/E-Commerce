using Services.Dtos.EmailDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.EmailServices
{
    public  interface IEmailServices
    {
        Task SendEmail(EmailDto dto);
    }
}
