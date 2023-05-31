using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models.Inventory;

public class Item
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public ItemType ItemType { get; set; }

    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Speed { get; set; }
    public int Health { get; set; }
    public string SpritePath  { get; set; }

    public List<UserInventory> UserInventories { get; set; }
}