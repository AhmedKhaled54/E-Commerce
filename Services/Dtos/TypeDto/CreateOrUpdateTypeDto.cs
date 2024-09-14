using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.TypeDto
{
    public  class CreateOrUpdateTypeDto
    {
        [Required(ErrorMessage ="Please Enter Type Name !")]
        public string TypeName { get; set; }
    }
}
