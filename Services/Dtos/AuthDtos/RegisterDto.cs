using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.AuthDtos
{
    public class RegisterDto
    {
        [Required(ErrorMessage ="Please Enter Your Name!")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please Enter Your Email!")]
        [DataType(DataType.EmailAddress,ErrorMessage = "Please Enter Your Email correctly!")]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        [Required(ErrorMessage ="Incorrect Password !")]
        [DataType(DataType.Password)]
       
        public string Password { get; set; }
        [Required]
        [Compare(nameof(Password),ErrorMessage = "Password Not Matching !")]
        [Display(Name ="Repeat Pawword ")]
        public string ConfirmPassword { get; set; }

    }
}
