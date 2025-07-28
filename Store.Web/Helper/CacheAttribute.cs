using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Store.Services.Services.CacheServices;
using System.Text;

namespace Store.Web.Helper
{
    public class CacheAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _seconds;

        public CacheAttribute(int seconds)
        {
            this._seconds = seconds;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var _cacheService = context.HttpContext.RequestServices.GetService(typeof(ICacheService)) as ICacheService;
            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);

            var cachedResponse = await _cacheService.GetCacheResponse(cacheKey);

            if (!string.IsNullOrEmpty(cachedResponse))
            {
                var contentResult = new ContentResult
                {
                    Content = cachedResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;
            }
            else
            {
                var executedContext = await next();
                if (executedContext.Result is ObjectResult okObjectResult)
                {
                    await _cacheService.SetAsync(cacheKey, okObjectResult.Value, TimeSpan.FromSeconds(_seconds));
                }
            }
        }
        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();

            keyBuilder.Append($"{request.Path}");

            foreach (var item in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"|{item.Key}-{item.Value}");
            }
            return keyBuilder.ToString();
        }
    }
}
