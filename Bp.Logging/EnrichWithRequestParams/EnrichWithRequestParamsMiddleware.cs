using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Bp.Logging.EnrichWithRequestParams
{
    public class EnrichWithRequestParamsMiddleware
    {
        private readonly Logger<EnrichWithRequestParamsMiddleware> _logger;
        private readonly RequestDelegate _next;

        public EnrichWithRequestParamsMiddleware(Logger<EnrichWithRequestParamsMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public Task InvokeAsync(HttpContext context)
        {
            LogContext.PushProperty("transactionId", context.TraceIdentifier);
//            await context.Response.WriteAsync("------- Before ------ \n\r");
//            await _next(context);

//            await context.Response.WriteAsync("\n\r------- After ------");
            return Task.CompletedTask;
        }
    }
}