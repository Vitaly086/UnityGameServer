using Server.Models;

namespace Server.Services.Interfaces;

public interface IInventoryService
{
    public InventoryResponse DeleteItem(int userId, int itemId);
    public InventoryResponse GetUserItems(int userId);
}