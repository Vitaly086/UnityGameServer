namespace Server.Models.Inventory;

public class InventoryItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<InventoryItemType> InventoryItemTypes { get; set; }

    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Speed { get; set; }
    public int Health { get; set; }

    public List<UserInventory> UserInventories { get; set; }
}