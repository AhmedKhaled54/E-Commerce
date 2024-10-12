using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.RateDtos
{
    public class UserRateDto
    {
        public int? Rate {  get; set; }
        public string?Comment {  get; set; }
        public string?UserName { get; set; }
    }
}
