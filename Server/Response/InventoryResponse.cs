using Newtonsoft.Json;
using Server.Extensions;
using Server.Models.Inventory;

namespace Server.Models;

public class InventoryResponse
{
    public bool Success { get; init; }
    public string? Content { get; init; }
    
    [JsonConverter(typeof(DictionaryJsonConverter<Item, int>))]
    public Dictionary<Item, int> UserItems { get; init; }
}