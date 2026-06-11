using System.Collections.Concurrent;

namespace QuoteFlowAI.Middleware
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly ConcurrentDictionary<string, (int Count, DateTime WindowStart)> _requests = new();
        private readonly int _limit = 100;
        private readonly TimeSpan _window = TimeSpan.FromMinutes(1);

        public RateLimitingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var now = DateTime.UtcNow;
            var key = ip;

            if (_requests.TryGetValue(key, out var entry))
            {
                if (now - entry.WindowStart > _window)
                {
                    // Reset window
                    _requests[key] = (1, now);
                }
                else if (entry.Count >= _limit)
                {
                    context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    await context.Response.WriteAsync("Rate limit exceeded. Try again later.");
                    return;
                }
                else
                {
                    _requests[key] = (entry.Count + 1, entry.WindowStart);
                }
            }
            else
            {
                _requests.TryAdd(key, (1, now));
            }

            await _next(context);
        }
    }
}