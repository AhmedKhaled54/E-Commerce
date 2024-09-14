using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.ProductsDto
{
    public class CreateProductDto:BaseProductDto
    {
        [Required]
        public IFormFile Image {  get; set; }


    }
}
