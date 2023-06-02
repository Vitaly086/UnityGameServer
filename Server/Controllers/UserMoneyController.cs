using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Request;
using Server.Response;
using Server.Services.Interfaces;

namespace Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserMoneyController : ControllerBase
{
    private readonly IUserMoneyService _userMoneyService;

    public UserMoneyController(IUserMoneyService userMoneyService)
    {
        _userMoneyService = userMoneyService;
    }

    [HttpPost("add-money")]
    public IActionResult AddMoney(UserMoneyRequest request)
    {
        var userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "id")?.Value);

        var result = _userMoneyService.TryAddMoney(userId, request);

       if (result.Success)
       {
           return Ok(result);
       }

       return BadRequest(result.Content);
    }
}