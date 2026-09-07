using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CCMSApp.WebApi.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    private ISender? _mediator;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    protected string CorrelationId => HttpContext.TraceIdentifier;
}
