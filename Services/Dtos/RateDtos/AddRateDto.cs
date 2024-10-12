using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.RateDtos
{
    public  class AddRateDto
    {
        [Range(1,5)]
        public int? Rate { get; set; } 
        public string?Message { get; set; }
        public int ProductId { get; set; }
    }
}
