using Server.Models;

namespace Server.Services.Interfaces;

public interface IInventoryService
{
    public InventoryResponse AddItemToUser(int userId, int itemId);
    public InventoryResponse GetItem(int userId, int itemId);
    public InventoryResponse DeleteItem(int userId, int userItemId);
    public InventoryResponse GetUserItems(int userId);
    public InventoryResponse GetAllGameItems();

}