using Serilog.Context;

namespace CCMSApp.WebApi.Middleware;

/// <summary>
/// Ensures every request has a correlation ID for distributed tracing.
/// </summary>
public sealed class CorrelationIdMiddleware : IMiddleware
{
    public const string CorrelationIdHeader = "X-Correlation-ID";

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var correlationId = context.Request.Headers.TryGetValue(CorrelationIdHeader, out var headerValue)
            ? headerValue.ToString()
            : Guid.NewGuid().ToString();

        context.TraceIdentifier = correlationId;
        context.Response.Headers[CorrelationIdHeader] = correlationId;

        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            await next(context);
        }
    }
}
