using Microsoft.AspNetCore.SignalR;

public class RequestCounterMiddleware
{
    private readonly RequestDelegate _next;
    private static int _activeRequests = 0;

    public RequestCounterMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        Interlocked.Increment(ref _activeRequests);

        try
        {
            await _next(context);
        }
        finally
        {
            Interlocked.Decrement(ref _activeRequests);
        }
    }

    public static int GetActiveRequests() => _activeRequests;
}
