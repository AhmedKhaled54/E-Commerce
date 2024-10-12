using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Caching
{
    public interface ICachServices
    {
        Task<string> GetResponse(string key);
        Task SetResponse(string key, object response, TimeSpan timelive);
    }
}
