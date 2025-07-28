using Store.Services.Handle_Responses;
using System.Net;
using System.Text.Json;

namespace Store.Web.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> logger;
        private readonly IHostEnvironment hostEnvironment;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment hostEnvironment)
        {
            this.next = next;
            this.logger = logger;
            this.hostEnvironment = hostEnvironment;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var response = hostEnvironment.IsDevelopment() ? new Custom_Exception(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                    : new Custom_Exception(context.Response.StatusCode, "Internal Server Error");
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.KebabCaseUpper
                };

                var json = JsonSerializer.Serialize(response, options);
                await context.Response.WriteAsync(json);
            };
        }
    }
}
