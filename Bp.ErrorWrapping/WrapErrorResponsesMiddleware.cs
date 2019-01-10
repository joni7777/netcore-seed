using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Bp.ErrorWrapping
{
    public class WrapErrorResponsesMiddleware
    {
        private readonly RequestDelegate _next;

        public WrapErrorResponsesMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception exception)
            {
                Guid errorId = Guid.NewGuid();

                var result = JsonConvert.SerializeObject(new { error = "Sorry, an unexpected error has occurred", errorId = errorId });
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync(result);
            }
        }
    }
}