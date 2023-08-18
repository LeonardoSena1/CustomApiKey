using CustomApiKey.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Net;

namespace CustomApiKey.Attribute
{
    [AttributeUsage(validOn: AttributeTargets.Class)]
    public class ApiKeyAttribute : System.Attribute, IAsyncActionFilter
    {
        private const string APIKEYNAME = "ApiKey";
        private const string NotProvided = "Api Key was not provided";
        private const string NotValid = "Api Key is not valid";
        private const string ContentType = "application/json";
        private const int StatusCode = (int)HttpStatusCode.Unauthorized;
        private const bool Success = false;
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = StatusCode,
                    ContentType = ContentType,
                    Content = JsonConvert.SerializeObject(
                        new BaseResponse
                        {
                            Success = Success,
                            Message = NotProvided,
                        })
                };
                return;
            }

            var appSettings = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();

            var apiKey = appSettings.GetValue<string>(APIKEYNAME);

            if (!apiKey.Equals(extractedApiKey))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = StatusCode,
                    ContentType = ContentType,
                    Content = JsonConvert.SerializeObject(
                        new BaseResponse
                        {
                            Success = Success,
                            Message = NotValid,
                        })
                };
                return;
            }

            await next();
        }
    }
}
