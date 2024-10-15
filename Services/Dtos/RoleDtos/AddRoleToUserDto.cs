﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.RoleDtos
{
    public  class AddRoleToUserDto
    {
        [Required(ErrorMessage ="UserId Is Required")]
        public string UserId {  get; set; }
        [Required(ErrorMessage ="Please Enter RoleName!")]
        public string RoleName  { get; set; }

    }
}
