using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Services.Helper
{
    public  class AuthModel
    {
        public string Token { get; set; }
        public string UserName { get; set; }
        public string Email  { get; set; }
        public bool IsAuthenticated { get; set; }
        public string Message { get; set; }
        public List<string> Roles  { get; set; }
        [JsonIgnore]
        public string RefreshToken {  get; set; }

        public DateTime RefreshTokenExpire { get; set; }
    }
}
