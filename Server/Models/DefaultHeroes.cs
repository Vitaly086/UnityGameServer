using System.ComponentModel.DataAnnotations;

namespace Server.Models;

public class DefaultHeroes
{
    [Key]
    public int HeroId { get; set; }
    public string Name { get; set; }
    public int Level { get; set; }
    public float Experience { get; set; }
    public string Description { get; set; }
    public float Health { get; set; }
    public float Attack { get; set; }
    public float Defense { get; set; }
    public float Speed { get; set; }
    public HeroType Type { get; set; }
    public bool WasBought { get; set; }
    public int Price { get; set; }
    public bool IsSelected { get; set; }
}