using Server.Models.Inventory;

namespace Server.Services.Interfaces;

public interface IInventoryService
{
    public void AddItemToUser(int userId, int itemId);
    public void AddItemToUser(int userId, int itemId, int quantity);
    public InventoryItem GetItem(int userId, int itemId);
    public void DeleteItem(int userId, int itemId);
    public List<InventoryItem> GetItemsByType(int userId, ItemType type);
    public List<UserInventory> GetUserItems(int userId);

}