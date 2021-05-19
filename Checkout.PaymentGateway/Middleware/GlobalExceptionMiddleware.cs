using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OpenTracing.Util;

namespace Checkout.PaymentGateway.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var traceId = GlobalTracer.Instance?.ActiveSpan?.Context.TraceId;

            try
            {
                await _next.Invoke(context);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, ex.Message);

                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync($"Error executing the request. Use '{traceId}' to find the error.");
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex, ex.Message);

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync($"Error executing the request. Use '{traceId}' to find the error.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync($"Error executing the request. Use '{traceId}' to find the error.");
            }
        }
    }
}
