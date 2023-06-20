using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;

namespace WebApi.Controllers;

[ApiController]
[ApiExceptionFilter]
public abstract class ApiControllerBase : ControllerBase
{
}
