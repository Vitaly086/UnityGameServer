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
            var userId = AuthenticationHelpers.GetUserIdFromHeader(Request.Headers);
            _inventoryService.AddItemToUser(userId, itemId);
            return Ok("Item added successfully.");
        }

        [HttpGet("add/id={itemId}&quantity={quantity}")]
        public IActionResult AddItemToUser(int itemId, int quantity)
        {
            var userId = AuthenticationHelpers.GetUserIdFromHeader(Request.Headers);
            _inventoryService.AddItemToUser(userId, itemId, quantity);
            return Ok($"Added {quantity} items successfully.");
        }

        [HttpGet("item/id={itemId}")]
        public IActionResult GetItem(int itemId)
        {
            var userId = AuthenticationHelpers.GetUserIdFromHeader(Request.Headers);
            try
            {
                var item = _inventoryService.GetItem(userId, itemId);
                return Ok(item);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("delete/id={itemId}")]
        public IActionResult DeleteItem(int itemId)
        {
            var userId = AuthenticationHelpers.GetUserIdFromHeader(Request.Headers);
            _inventoryService.DeleteItem(userId, itemId);
            return Ok("Item deleted successfully.");
        }

        [HttpGet("items")]
        public IActionResult GetUserItems()
        {
            var userId = AuthenticationHelpers.GetUserIdFromHeader(Request.Headers);
            var userItems = _inventoryService.GetUserItems(userId);
            return Ok(userItems);
        }

        [HttpGet("items/type={type}")]
        public IActionResult GetItemsByType(ItemType type)
        {
            var userId = AuthenticationHelpers.GetUserIdFromHeader(Request.Headers);
            var items = _inventoryService.GetItemsByType(userId, type);
            return Ok(items);
        }
    }
}