using Server.Models.Inventory;

namespace Server.Response;

public class ShopResponse
{
    public bool Success { get; init; }

    public string? Content { get; set; }
    public Item Item { get; set; }
    public List<Item> GameItems { get; set; }
}