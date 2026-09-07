using CCMSApp.Core.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CCMSApp.WebApi.Middleware;

/// <summary>
/// Catches unhandled exceptions and returns RFC 7807 ProblemDetails responses.
/// </summary>
public sealed class GlobalExceptionMiddleware : IMiddleware
{
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly IWebHostEnvironment _environment;

    public GlobalExceptionMiddleware(
        ILogger<GlobalExceptionMiddleware> logger,
        IWebHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred while processing request {TraceId}", context.TraceIdentifier);
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/problem+json";

        var (statusCode, title, detail) = exception switch
        {
            NotFoundException notFound => ((int)HttpStatusCode.NotFound, "Not Found", notFound.Message),
            ValidationException validation => ((int)HttpStatusCode.BadRequest, "Bad Request", validation.Message),
            _ => ((int)HttpStatusCode.InternalServerError, "Internal Server Error", "An unexpected error occurred.")
        };

        context.Response.StatusCode = statusCode;

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = _environment.IsDevelopment() ? exception.Message : detail,
            Instance = context.Request.Path,
            Extensions =
            {
                ["correlationId"] = context.TraceIdentifier,
                ["traceId"] = context.TraceIdentifier
            }
        };

        if (exception is ValidationException validationException && validationException.Errors.Any())
        {
            problem.Extensions["errors"] = validationException.Errors;
        }

        await context.Response.WriteAsJsonAsync(problem);
    }
}
