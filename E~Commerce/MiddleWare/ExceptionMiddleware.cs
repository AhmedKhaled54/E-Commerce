
using Services.HandleResponse;
using System.Net;
using System.Text.Json;

namespace E_Commerce.MiddleWare
{
    public class ExceptionMiddleware
    {
        private ILogger<ExceptionMiddleware> logger;
        private RequestDelegate _next;
        private IWebHostEnvironment _environment;   
      
        

        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, RequestDelegate next, IWebHostEnvironment environment)
        {
            this.logger = logger;
            _next = next;
            _environment = environment;
        }


        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode=(int)HttpStatusCode.InternalServerError;

                var response = _environment.IsDevelopment()
                    ? new ApiException((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
                    : new ApiException((int)HttpStatusCode.InternalServerError);
                
                var option = new JsonSerializerOptions {PropertyNamingPolicy=JsonNamingPolicy.CamelCase};
                var jsonserialize=JsonSerializer.Serialize(response, option);
                
                await context.Response.WriteAsync(jsonserialize);

            }

        }
    }
}
