
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services.Caching
{
    public class CachServices : ICachServices
    {
        private readonly IDatabase _database;
        public CachServices(IConnectionMultiplexer redis)
        {
             _database = redis.GetDatabase();
        }
        public async Task<string> GetResponse(string key)
        {
            var resposne=await _database.StringGetAsync(key);
            if (resposne.IsNullOrEmpty)
                return null;

            return resposne;

        }

        public async  Task SetResponse(string key, object response, TimeSpan timelive)
        {
            if (response is null)
                return;
            var Option=new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var ResponseSerailize=JsonSerializer.Serialize(response,Option);
            await _database.StringSetAsync(key,ResponseSerailize,timelive);
        }
    }
}
