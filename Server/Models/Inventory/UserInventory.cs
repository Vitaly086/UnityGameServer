namespace Server.Models.Inventory;

public class UserInventory
{
    public int UserId { get; set; }
    public UserProfile User { get; set; }

    public int ItemId { get; set; }
    public InventoryItem Item { get; set; }

    public int Quantity { get; set; }
}