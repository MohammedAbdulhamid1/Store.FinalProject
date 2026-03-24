using Microsoft.AspNetCore.Mvc.Abstractions;
using NPoco.fastJSON;
using Store.FinalProject.Errors;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.FinalProject.MiddleWares
{
    public class ExeptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExeptionMiddleware> logger;
        private readonly IHostEnvironment env;

        public ExeptionMiddleware(RequestDelegate next,ILogger<ExeptionMiddleware> logger,IHostEnvironment env)
        {
            this.next = next;
            this.logger = logger;
            this.env = env;
        }
        public async Task Invoke (HttpContext context)
        {
            try
            {
                await next.Invoke(context);
            }
            catch(Exception ex)
            {
                logger.LogError(ex, ex.Message);
                context.Response.Clear();
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                var response = env.IsDevelopment() ?
                    new ApiExeptionResponse(StatusCodes.Status500InternalServerError, ex.Message, ex?.StackTrace?.ToString())
                    :new ApiExeptionResponse(StatusCodes.Status500InternalServerError)
                    ;
                var json = JsonSerializer.Serialize(response);
               await context.Response.WriteAsync(json);
            }
        }

    }
}
