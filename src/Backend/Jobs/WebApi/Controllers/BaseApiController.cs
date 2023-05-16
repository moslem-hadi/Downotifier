using Microsoft.AspNetCore.Mvc;
using WebUI.Filters;

namespace WebApi.Controllers;

[ApiController]
[ApiExceptionFilter]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    private ISender? _mediator;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}
