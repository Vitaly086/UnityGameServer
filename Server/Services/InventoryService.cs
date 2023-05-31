using Microsoft.EntityFrameworkCore;
using Server.Models;
using Server.Models.Inventory;
using Server.Services.Interfaces;

namespace Server.Services;

public class InventoryService : IInventoryService
{
    private readonly GameDbContext _context;

    public InventoryService(GameDbContext context)
    {
        _context = context;

        if (!_context.Items.Any())
        {
            CreateDefaultItems();
        }
    }

    public InventoryResponse AddItemToUser(int userId, int itemId)
    {
        var item = _context.Items.Find(itemId);
        if (item == null)
        {
            return new InventoryResponse()
            {
                Content = "Item not found",
                Success = false
            };
        }

        var userInventory = new UserInventory
        {
            UserId = userId,
            Item = item,
        };
        _context.UserInventories.Add(userInventory);

        _context.SaveChanges();
        return new InventoryResponse()
        {
            Content = "Item added successfully",
            Success = true,
            UserItem = new UserItem()
            {
                UserItemId = userInventory.UserInventoryId,
                Item = userInventory.Item
            }
        };
    }


    public InventoryResponse GetItem(int userId, int itemId)
    {
        var userInventory = _context.UserInventories
            .Include(ui => ui.Item)
            .SingleOrDefault(ui => ui.UserId == userId && ui.ItemId == itemId);

        if (userInventory?.Item == null)
        {
            return new InventoryResponse()
            {
                Success = false,
                Content = "Item not found"
            };
        }

        return new InventoryResponse()
        {
            Success = true,
            UserItem = new UserItem()
            {
                UserItemId = userInventory.UserInventoryId,
                Item = userInventory.Item
            }
        };
    }

    public InventoryResponse DeleteItem(int userId, int userItemId)
    {
        var userInventory = _context.UserInventories
            .SingleOrDefault(ui => ui.UserId == userId && ui.UserInventoryId == userItemId);

        if (userInventory == null)
        {
            return new InventoryResponse()
            {
                Success = false,
                Content = "Item not found"
            };
        }


        _context.UserInventories.Remove(userInventory);

        _context.SaveChanges();
        var userItems = _context.UserInventories
            .Where(ui => ui.UserId == userId)
            .Select(ui => new
            {
                ui.UserInventoryId,
                ui.Item
            })
            .ToList();

        if (userItems.Count == 0)
        {
            return new InventoryResponse()
            {
                Success = false,
                Content = "All item deleted"
            };
        }

        var itemsWithInventoryId = userItems.Select(ui => new UserItem()
        {
            UserItemId = ui.UserInventoryId,
            Item = ui.Item
        }).ToList();

        return new InventoryResponse()
        {
            Success = true,
            Content = "Item deleted",
            UserItems = itemsWithInventoryId
        };
    }

    public InventoryResponse GetUserItems(int userId)
    {
        var userItems = _context.UserInventories
            .Where(ui => ui.UserId == userId)
            .Select(ui => new
            {
                ui.UserInventoryId,
                ui.Item
            })
            .ToList();

        if (userItems.Count == 0)
        {
            return new InventoryResponse()
            {
                Success = false,
                Content = "Items not found"
            };
        }

        var itemsWithInventoryId = userItems.Select(ui => new UserItem()
        {
            UserItemId = ui.UserInventoryId,
            Item = ui.Item
        }).ToList();

        return new InventoryResponse()
        {
            Success = true,
            UserItems = itemsWithInventoryId
        };
    }

    public InventoryResponse GetAllGameItems()
    {
        var items = _context.Items.ToList();

        if (items.Count == 0)
        {
            return new InventoryResponse()
            {
                Success = false,
                Content = "Items not found"
            };
        }

        return new InventoryResponse()
        {
            Success = true,
            GameItems = items
        };
    }

    private void CreateDefaultItems()
    {
        var sword = new Item()
        {
            Id = 1,
            Name = "Sword",
            Description = "Base sword",
            ItemType = ItemType.Weapon,
            Attack = 10,
            Defense = 0,
            Speed = 0,
            Health = 0,
            SpritePath = "Assets/Resources/Sprites/Demo/Demo_Icon_ItemIcons(Original)/Icon_Sword.Png"
        };

        var shield = new Item()
        {
            Id = 2,
            Name = "Shield",
            Description = "Base shield",
            ItemType = ItemType.Armor,
            Attack = 0,
            Defense = 10,
            Speed = 0,
            Health = 0,
            SpritePath = "Assets/Resources/Sprites/Demo/Demo_Icon_ItemIcons(Original)/Icon_Shield.Png"
        };

        var boots = new Item()
        {
            Id = 3,
            Name = "Boots",
            Description = "Base boots",
            ItemType = ItemType.Movement,
            Attack = 0,
            Defense = 2,
            Speed = 10,
            Health = 0,
            SpritePath = "Assets/Resources/Sprites/Demo/Demo_Icon_ItemIcons(Original)/Icon_Boots.Png"
        };

        var crown = new Item()
        {
            Id = 4,
            Name = "Crown",
            Description = "Crown of the best king",
            ItemType = ItemType.Accessory,
            Attack = 0,
            Defense = 2,
            Speed = 3,
            Health = 15,
            SpritePath = "Assets/Resources/Sprites/Demo/Demo_Icon_ItemIcons(Original)/Icon_Crown.Png"
        };

        var compas = new Item()
        {
            Id = 5,
            Name = "Compas",
            Description = "Magic compas",
            ItemType = ItemType.Accessory,
            Attack = 0,
            Defense = 0,
            Speed = 5,
            Health = 1,
            SpritePath = "Assets/Resources/Sprites/Demo/Demo_Icon_ItemIcons(Original)/Icon_Compass.Png"
        };

        var doubleSwords = new Item()
        {
            Id = 6,
            Name = "Double swords ",
            Description = "Double swords of master",
            ItemType = ItemType.Weapon,
            Attack = 15,
            Defense = 0,
            Speed = 0,
            Health = 0,
            SpritePath = "Assets/Resources/Sprites/Demo/Demo_Icon_ItemIcons(Original)/Icon_Battle.Png"
        };
        _context.Items.AddRange(sword, shield, boots, crown, compas, doubleSwords);
        _context.SaveChanges();
    }
}