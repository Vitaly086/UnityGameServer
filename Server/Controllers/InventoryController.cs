using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Models.Inventory;
using Server.Services.Interfaces;
using Server.Extensions;

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

        [HttpGet("add/id={itemId}")]
        public IActionResult AddItemToUser(int itemId)
        {
            var userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "id")?.Value);

            var result = _inventoryService.AddItemToUser(userId, itemId);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result.Content);
        }


        [HttpGet("item/id={itemId}")]
        public IActionResult GetItem(int itemId)
        {
            var userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "id")?.Value);

            var result = _inventoryService.GetItem(userId, itemId);

            if (result.Success)
            {
                return Ok(result);
            }


            return BadRequest(result.Content);
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
        
        [HttpGet("items/all")]
        public IActionResult GetAllGameItems()
        {
            var result = _inventoryService.GetAllGameItems();

            if (result.Success)
            {
                return Ok(result);
            }

            return NotFound(result.Content);
        }
    }
}