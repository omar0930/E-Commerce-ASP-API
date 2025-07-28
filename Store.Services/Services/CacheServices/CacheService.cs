using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Services.Services.CacheServices
{
    public class CacheService : ICacheService
    {
        private readonly IDatabase _database;
        public CacheService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task<string> GetCacheResponse(string key)
        {
            var cachedResponse = await _database.StringGetAsync(key);

            if (!cachedResponse.HasValue)
            {
                return null;
            }
            else
            {
                return cachedResponse.ToString();
            }
        }

        public async Task SetAsync(string key, object response, TimeSpan? expiration = null)
        {
            if (response is not null)
            {
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var serializedResponse = JsonSerializer.Serialize(response, options);

                await _database.StringSetAsync(key, serializedResponse, expiration);
            }
        }
    }
}
