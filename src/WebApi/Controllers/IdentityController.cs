using Infrastructure.Common;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;
[ApiVersion("1.0")]

[Route("api/account")]
public class IdentityController : ApiControllerBase
{
    private readonly IIdentityService _identityService;
    
    public IdentityController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    [HttpPost]
    public async Task<ActionResult<string>> Login()
    {
        await _identityService.SignIn();
        return Ok("ok");
    }
}