using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.ApiController;

[ApiController]
[ApiVersion("1.0")]
[Route("/api/test/test")]
public class TestSecController: ControllerBase
{
    [HttpGet]
    [Route("out")]
    [Authorize]
    public ActionResult<string> Test()
    {
        return Ok("ok");
    }
    
    [HttpGet]
    [Route("in")]
    public ActionResult<string> TestIn()
    {
        return Ok("ok");
    }
}