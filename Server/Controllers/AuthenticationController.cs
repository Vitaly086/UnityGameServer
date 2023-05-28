using Microsoft.AspNetCore.Mvc;
using Server.Request;
using Server.Response;
using Server.Services.Interfaces;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("registration")]
    public IActionResult Register(AuthenticationRequest request)
    {
        var result = _authenticationService.Register(request);
        return result.Success ? Login(request) : BadRequest(result.Content);
    }

    [HttpPost("login")]
    public IActionResult Login(AuthenticationRequest request)
    {
        var result = _authenticationService.Login(request);
        if (result.Success)
        {
            return Ok(new AuthenticationResponse()
            {
                UserProfile = result.UserProfile
            });
        }

        return BadRequest(result.Content);
    }
}