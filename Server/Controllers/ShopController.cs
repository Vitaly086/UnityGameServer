using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ShopController : ControllerBase
{
    private readonly IShopService _shopService;

    public ShopController(IShopService shopService)
    {
        _shopService = shopService;
    }
        
    [HttpPut("buy/id={itemId}")]
    public IActionResult AddItemToUser(int itemId)
    {
        var userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "id")?.Value);

        var result = _shopService.TryBuyItemToUser(userId, itemId);
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result.Content);
    }
    
    [HttpGet("items")]
    public IActionResult GetAllGameItems()
    {
        var result = _shopService.GetAllGameItems();

        if (result.Success)
        {
            return Ok(result);
        }

        return NotFound(result.Content);
    }
}