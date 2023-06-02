using Server.Models.Inventory;
using Server.Response;
using Server.Services.Interfaces;

namespace Server.Services;

public class ShopService : IShopService
{
    private readonly GameDbContext _context;
    private readonly IUserMoneyService _userMoneyService;

    public ShopService(GameDbContext context, IUserMoneyService userMoneyService)
    {
        _context = context;
        _userMoneyService = userMoneyService;
    }

    public ShopResponse TryBuyItemToUser(int userId, int itemId)
    {
        var item = _context.Items.Find(itemId);
        if (item == null)
        {
            return new ShopResponse()
            {
                Content = "Item not found",
                Success = false
            };
        }

        var user = _context.UserProfiles.Find(userId);
        if (user == null)
        {
            return new ShopResponse()
            {
                Content = "User not found",
                Success = false
            };
        }

        if (!_userMoneyService.CheckUserHasEnoughMoney(userId, item.Price))
        {
            return new ShopResponse()
            {
                Content = "Insufficient funds",
                Success = false
            };
        }

        var userInventories = _context.UserInventories
            .Where(ui => ui.UserId == userId && ui.ItemId == itemId)
            .ToList();

        UserInventory userInventory;

        if (userInventories.Count > 0)
        {
            userInventory = userInventories[0];
            userInventory.Count += userInventories.Skip(1).Sum(ui => ui.Count) + 1;

            _context.UserInventories.RemoveRange(userInventories.Skip(1));
        }
        else
        {
            userInventory = new UserInventory
            {
                UserId = userId,
                ItemId = itemId,
                Count = 1
            };
            _context.UserInventories.Add(userInventory);
        }

        _userMoneyService.DeductMoneyFromUser(userId, item.Price);

        _context.SaveChanges();

        return new ShopResponse()
        {
            Content = "Item added successfully",
            Success = true,
            Item = item,
        };
    }
    
    public ShopResponse GetAllGameItems()
    {
        var items = _context.Items.ToList();

        if (items.Count == 0)
        {
            return new ShopResponse()
            {
                Success = false,
                Content = "Items not found"
            };
        }

        return new ShopResponse()
        {
            Success = true,
            GameItems = items
        };
    }
}