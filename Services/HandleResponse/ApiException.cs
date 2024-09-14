using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.HandleResponse
{
    public class ApiException : ApiResponse
    {
        public ApiException(int status, string message=null,string Details=null ) 
            : base(status, message)
        {
            this.Details = Details;
        }

        public string Details { get; set; }
    }
}
