using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client.AuthScheme;

namespace WebApi.Controllers;

[ApiController]

[Route("/api/test")]
public class TestController: ControllerBase
{
    [HttpGet]
    [Route("get-in")]
    [Authorize]
    public ActionResult<string> GetIn()
    {
        return Ok("ok");
    }
    
    [HttpGet]
    [Route("get-out")]
    public ActionResult<string> GetOut()
    {
        return Ok("ok");
    }
}