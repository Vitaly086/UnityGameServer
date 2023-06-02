using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Services;

public class HeroesService
{
    private readonly GameDbContext _context;

    public HeroesService(GameDbContext context)
    {
        _context = context;
    }

    public HeroesSettings CreateDefaultHero(int heroId)
    {
        var defaultHero = _context.DefaultHeroes.FirstOrDefault(h => h.HeroId == heroId);
        if (defaultHero == null)
        {
            defaultHero = new DefaultHeroes
            {
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

        var hero = new HeroesSettings
        {
            PrefabId = defaultHero.PrefabId,
            Name = defaultHero.Name,
            Level = defaultHero.Level,
            Experience = defaultHero.Experience,
            Description = defaultHero.Description,
            Health = defaultHero.Health,
            Attack = defaultHero.Attack,
            Defense = defaultHero.Defense,
            Speed = defaultHero.Speed,
            Type = defaultHero.Type,
            WasBought = defaultHero.WasBought,
            Price = defaultHero.Price,
            IsSelected = defaultHero.IsSelected
        };

        return hero;
    }

    public List<HeroesSettings> GetUserHeroes(int userId)
    {
        return _context.HeroesSettings.Include(hero => hero.UserProfile)
            .Where(hero => hero.UserProfile.Id == userId)
            .ToList();
    }
}