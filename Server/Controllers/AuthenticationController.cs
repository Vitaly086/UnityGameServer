using Microsoft.AspNetCore.Mvc;
using Server.Request;
using Server.Response;
using Server.Services;

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

    [HttpPost("register")]
    public IActionResult Register(AuthenticationRequest request)
    {
        var (success, content) = _authenticationService.Register(request);
        return success ? Login(request) : BadRequest(content);
    }

    [HttpPost("login")]
    public IActionResult Login(AuthenticationRequest request)
    {
        var (success, content, userProfile) = _authenticationService.Login(request);
        if (success)
        {
            return Ok(new AuthenticationResponse()
            {
                UserProfile = userProfile
            });
        }

        return BadRequest(content);
    }
}