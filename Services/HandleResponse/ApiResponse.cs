using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.HandleResponse
{
    public  class ApiResponse
    {
        public ApiResponse(int status ,string message )
        {
            StatusCode = status;
            Message = message??GetDefoultMessage(status);
        }

        public int StatusCode {  get; set; }
        public string Message { get; set; }

        private string GetDefoultMessage(int code)
            => code switch
            {
                400 => "BadRequest!",
                401 => "Not Authorized!",
                404 => "Not Found!",
                500 => "Internal Server Error!",
                _ => null

            };
        


    }
}
