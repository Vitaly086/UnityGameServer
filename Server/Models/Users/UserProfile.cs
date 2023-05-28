using System.ComponentModel.DataAnnotations.Schema;
using Server.Models.Inventory;

namespace Server.Models;

public class UserProfile
{
    public int Id { get; set; }
    public string? JwtToken { get; set; }
    public string? DeviceId { get; set; }

    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string Salt { get; set; }
    public int Money { get; set; } = 1000;
    public int Gems { get; set; } = 100;

    [ForeignKey("UserId")] 
    public List<HeroesSettings> HeroesSettings { get; set; } = new();
    
    public List<UserInventory> Inventory { get; set; }

}