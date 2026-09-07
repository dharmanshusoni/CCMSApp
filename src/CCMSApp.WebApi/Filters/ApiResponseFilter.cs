using CCMSApp.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CCMSApp.WebApi.Filters;

/// <summary>
/// Wraps successful JSON results in the standard ApiResponse&lt;T&gt; envelope.
/// </summary>
public sealed class ApiResponseFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception is not null)
            return;

        var correlationId = context.HttpContext.TraceIdentifier;

        switch (context.Result)
        {
            case CreatedAtActionResult createdResult when createdResult.Value is not null:
            {
                var wrapped = WrapValue(createdResult.Value, correlationId);
                context.Result = new CreatedAtActionResult(
                    createdResult.ActionName,
                    createdResult.ControllerName,
                    createdResult.RouteValues,
                    wrapped)
                {
                    StatusCode = createdResult.StatusCode,
                    DeclaredType = wrapped.GetType(),
                    ContentTypes = createdResult.ContentTypes
                };
                break;
            }

            case ObjectResult objectResult when objectResult.Value is not null:
            {
                var wrapped = WrapValue(objectResult.Value, correlationId);
                context.Result = new ObjectResult(wrapped)
                {
                    StatusCode = objectResult.StatusCode,
                    DeclaredType = wrapped.GetType(),
                    ContentTypes = objectResult.ContentTypes
                };
                break;
            }
        }
    }

    private static object WrapValue(object value, string correlationId)
    {
        return Activator.CreateInstance(
            typeof(ApiResponse<>).MakeGenericType(value.GetType()),
            value,
            correlationId)!;
    }
}
