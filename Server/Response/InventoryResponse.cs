using Server.Models.Inventory;

namespace Server.Models;

public class InventoryResponse
{
    public bool Success { get; init; }
    public string? Content { get; init; }
    public UserItem UserItem { get; init; }
    public List<UserItem> UserItems { get; init; }
    public List<Item> GameItems { get; init; }
}