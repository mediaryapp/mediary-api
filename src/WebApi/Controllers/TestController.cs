using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("/api/test")]
public class TestController : ApiControllerBase
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