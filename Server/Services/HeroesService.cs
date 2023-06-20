using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Services;

public class HeroesService
{
    private readonly GameDbContext _context;

    public HeroesService(GameDbContext context)
    {
        _context = context;

        if (!_context.DefaultHeroes.Any())
        {
            CreateDefaultHero();
        }
    }

    private void CreateDefaultHero()
    {
        var defaultHero = new DefaultHeroes
        {
            HeroId = 1,
            PrefabId = 1,
            Name = "NoWeaponed",
            Level = 1,
            Experience = 0,
            Description = "Basic Hero",
            Health = 100,
            Attack = 10,
            Defense = 2,
            Speed = 5,
            Type = 0,
            WasBought = true,
            Price = 0,
            IsSelected = true
        };

        _context.Add(defaultHero);
        _context.SaveChanges();
    }

    public List<HeroesSettings> GetUserHeroes(int userId)
    {
        return _context.HeroesSettings.Include(hero => hero.UserProfile)
            .Where(hero => hero.UserProfile.Id == userId)
            .ToList();
    }
}