using Infrastructure.Common.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("/api/test")]
public class TestController : ApiControllerBase
{

    private readonly ICurrentUserService _currentUserService;
    public TestController(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }
    
    [HttpGet]
    [Route("get-in")]
    [Authorize]
    public ActionResult<string> GetIn()
    {
        var id =_currentUserService.UserId;
        return Ok("ok user-id:" + id);
    }

    [HttpGet]
    [Route("get-out")]
    public ActionResult<string> GetOut()
    {
        return Ok("ok");
    }
}