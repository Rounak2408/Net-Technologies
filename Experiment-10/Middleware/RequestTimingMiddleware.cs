using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Experiment_10.Middleware
{
    public class RequestTimingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestTimingMiddleware> _logger;

        public RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            context.Response.OnStarting(() =>
            {
                stopwatch.Stop();
                context.Response.Headers["X-Response-Time-ms"] = stopwatch.ElapsedMilliseconds.ToString();
                return Task.CompletedTask;
            });

            await _next(context);

            if (stopwatch.IsRunning)
            {
                stopwatch.Stop();
            }

            _logger.LogInformation("[Timing] {Method} {Path} executed in {ElapsedMilliseconds} ms",
                context.Request.Method,
                context.Request.Path,
                stopwatch.ElapsedMilliseconds);
        }
    }
}
