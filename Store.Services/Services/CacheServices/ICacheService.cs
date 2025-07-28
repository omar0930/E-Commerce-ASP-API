using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Services.CacheServices
{
    public interface ICacheService
    {
        Task SetAsync(string key, object response, TimeSpan? expiration = null);

        Task<string> GetCacheResponse (string key);
    }
}
