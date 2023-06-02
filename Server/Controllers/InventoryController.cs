using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }


        [HttpDelete("delete/id={itemId}")]
        public IActionResult DeleteItem(int itemId)
        {
            var userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "id")?.Value);
            var result = _inventoryService.DeleteItem(userId, itemId);

            if (result.Success)
            {
                return Ok(result);
            }

            return NotFound(result.Content);
        }

        [HttpGet("items")]
        public IActionResult GetUserItems()
        {
            var userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "id")?.Value);
            var result = _inventoryService.GetUserItems(userId);

            if (result.Success)
            {
                return Ok(result);
            }

            return NotFound(result.Content);
        }
    }
}