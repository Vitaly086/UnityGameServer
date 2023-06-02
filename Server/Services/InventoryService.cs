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

    public InventoryResponse DeleteItem(int userId, int itemId)
    {
        var userInventory = _context.UserInventories
            .SingleOrDefault(ui => ui.UserId == userId && ui.ItemId == itemId);

        if (userInventory == null)
        {
            return new InventoryResponse()
            {
                Success = false,
                Content = "Item not found"
            };
        }

        if (userInventory.Count > 1)
        {
            userInventory.Count--;
        }
        else
        {
            _context.UserInventories.Remove(userInventory);
        }

        _context.SaveChanges();

        return new InventoryResponse()
        {
            Success = true,
            Content = "Item deleted",
        };
    }

    public InventoryResponse GetUserItems(int userId)
    {
        var userItems = _context.UserInventories
            .Where(ui => ui.UserId == userId)
            .Include(ui => ui.Item)
            .GroupBy(ui => ui.Item)
            .ToDictionary(g =>
                g.Key, g =>
                g.Sum(ui => ui.Count));


        if (userItems.Count == 0)
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
            UserItems = userItems
        };
    }

    private void CreateDefaultItems()
    {
        var sword = new Item()
        {
            Id = 1,
            Name = "Sword",
            Description = "Base sword",
            Price = 10,
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
            Price = 200,
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
            Price = 600,
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
            Price = 1000,
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
            Price = 50,
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
            Price = 500,
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