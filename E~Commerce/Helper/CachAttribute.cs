using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Services.Caching;
using System.Text;

namespace E_Commerce.Helper
{
    public class CachAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int time;

        public CachAttribute(int Time)
        {
            time = Time;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var CachServices=context.HttpContext.RequestServices.GetRequiredService<ICachServices>();
            var CachKey=Generatekey(context.HttpContext.Request);

            var resposne = await CachServices.GetResponse(CachKey);
            if (!string.IsNullOrEmpty(resposne))
            {
                var contentResult = new ContentResult
                {
                    Content = resposne,
                    ContentType = "application/json",
                    StatusCode = 200,
                };
                context.Result = contentResult;
                return;
            }
        
            var Excuted =await next();

            if (Excuted.Result is OkObjectResult result)
                await CachServices.SetResponse(CachKey, result.Value, TimeSpan.FromMinutes(10));

        }




        private string Generatekey(HttpRequest request)
        {
            var CachKey = new StringBuilder();
            CachKey.Append($"{request.Path}");
            foreach(var (key,value) in request.Headers.OrderBy(k=>k.Key))
            {
                CachKey.Append($"{key}-{value}");
            }
            return CachKey.ToString();

        }
    }
}
