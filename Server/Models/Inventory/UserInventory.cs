using System.ComponentModel.DataAnnotations;

namespace Server.Models.Inventory;

public class UserInventory
{
    [Key] public int UserInventoryId { get; set; }

    public int UserId { get; set; }
    public UserProfile User { get; set; }
    public int ItemId { get; set; }
    public Item Item { get; set; }
    public int Count { get; set; }
}