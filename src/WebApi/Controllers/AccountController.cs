﻿using Infrastructure.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/account")]
public class AccountController : ApiControllerBase
{
    private readonly IIdentityService _identityService;

    public AccountController(IIdentityService identityService)
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