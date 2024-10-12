using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.RateDtos
{
    public class ProductRatedto
    {
        public int?Count { get; set; }
        public decimal?Avarage { get; set; }
        public List<UserRateDto>? CustomerRate{ get; set; }
    }
}
