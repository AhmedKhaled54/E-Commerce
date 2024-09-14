using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.Response
{
    public  class ResponseDto
    {
        public bool IsSucceeded { get; set; }
        public int  Status { get; set; }
        public string? Message { get; set; }
        public object?Model { get; set; }
        public IEnumerable<object>? Models {  get; set; }
    }
}
