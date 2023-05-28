namespace Server.Models.Inventory;

public class InventoryItemType
{
    public int Id { get; set; }
    public int InventoryItemId { get; set; }
    public ItemType ItemType { get; set; }

    public InventoryItem InventoryItem { get; set; }
}