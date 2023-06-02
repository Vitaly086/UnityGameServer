using Server.Response;

namespace Server.Services;

public interface IShopService
{
   public ShopResponse TryBuyItemToUser(int userId, int itemId);
   public  ShopResponse GetAllGameItems();
}