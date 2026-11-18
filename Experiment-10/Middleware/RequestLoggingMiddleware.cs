using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Experiment_10.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var method = context.Request.Method;
            var path = context.Request.Path;
            var timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff UTC");

            _logger.LogInformation("--> [Incoming Request] {Method} {Path} at {Timestamp}", method, path, timestamp);

            await _next(context);

            var statusCode = context.Response.StatusCode;
            _logger.LogInformation("<-- [Completed Response] {Method} {Path} responded with HTTP {StatusCode}", method, path, statusCode);
        }
    }
}
