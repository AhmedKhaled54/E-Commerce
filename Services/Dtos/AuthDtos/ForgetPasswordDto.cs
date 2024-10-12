using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.AuthDtos
{
    public  class ForgetPasswordDto
    {
        [Required(ErrorMessage = "Please Enter Your Email!")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please Enter Your Email correctly!")]
        public string Email { get; set; }
    }
}
