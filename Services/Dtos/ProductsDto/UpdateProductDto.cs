using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.ProductsDto
{
    public  class UpdateProductDto:BaseProductDto
    {
        public IFormFile?Image {  get; set; }
    }
}
