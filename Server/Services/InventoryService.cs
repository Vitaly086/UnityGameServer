using Microsoft.EntityFrameworkCore;
using Server.Models.Inventory;
using Server.Services.Interfaces;

namespace Server.Services;

public class InventoryService : IInventoryService
{
    private readonly GameDbContext _context;

    public InventoryService(GameDbContext context)
    {
        _context = context;
    }

    public void AddItemToUser(int userId, int itemId)
    {
        AddItemToUser(userId, itemId, 1);
    }

    public void AddItemToUser(int userId, int itemId, int quantity)
    {
        var userInventory = _context.UserInventories
            .SingleOrDefault(ui => ui.UserId == userId && ui.ItemId == itemId);

        if (userInventory != null)
        {
            // Если предмет уже есть в инвентаре, увеличьте количество
            userInventory.Quantity += quantity;
        }
        else
        {
            // Если предмета нет в инвентаре, добавьте новый
            var item = _context.InventoryItems.Find(itemId);
            if (item == null)
            {
                throw new Exception("Item not found");
            }

            userInventory = new UserInventory
            {
                UserId = userId,
                Item = item,
                Quantity = quantity
            };
            _context.UserInventories.Add(userInventory);
        }

        _context.SaveChanges();
    }


    public InventoryItem GetItem(int userId, int itemId)
    {
        var userInventory = _context.UserInventories
            .Include(ui => ui.Item) // Включаем связанный объект Item
            .ThenInclude(item => item.InventoryItemTypes) // Включаем связанный объект InventoryItemTypes
            .SingleOrDefault(ui => ui.UserId == userId && ui.ItemId == itemId);

        if (userInventory?.Item == null)
        {
            throw new Exception("Item not found");
        }

        return userInventory.Item;
    }

    public void DeleteItem(int userId, int itemId)
    {
        var userInventory = _context.UserInventories
            .SingleOrDefault(ui => ui.UserId == userId && ui.ItemId == itemId);

        if (userInventory == null)
        {
            throw new Exception("Item not found");
        }

        if (userInventory.Quantity > 1)
        {
            userInventory.Quantity--;
        }
        else
        {
            _context.UserInventories.Remove(userInventory);
        }

        _context.SaveChanges();
    }

    public List<InventoryItem> GetItemsByType(int userId, ItemType type)
    {
        var userInventoryItems = _context.UserInventories
            .Where(ui => ui.UserId == userId)
            .Select(ui => ui.ItemId)
            .ToList();

        var items = _context.InventoryItems
            .Include(item => item.InventoryItemTypes) // Включаем связанные данные InventoryItemTypes
            .Where(item => userInventoryItems.Contains(item.Id) &&
                           item.InventoryItemTypes.Any(iit => iit.ItemType == type))
            .ToList();

        return items;
    }

    public List<UserInventory> GetUserItems(int userId)
    {
        var userItems = _context.UserInventories
            .Where(ui => ui.UserId == userId)
            .Include(ui => ui.Item) // Включаем связанные данные Item
            .ThenInclude(item => item.InventoryItemTypes) // Включаем связанные данные InventoryItemTypes
            .ToList();

        return userItems;
    }
}