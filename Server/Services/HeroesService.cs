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

    public Heroes CreateDefaultHero(int heroId)
    {
        var defaultHero = _context.DefaultHeroes.FirstOrDefault(h => h.HeroId == heroId);

        if (defaultHero != null)
        {
            var hero = new Heroes
            {
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

        return null;
    }

    public List<Heroes> GetUserHeroes(int userId)
    {
        return _context.Heroes.Include(hero => hero.UserProfile)
            .Where(hero => hero.UserProfile.Id == userId)
            .ToList();
    }
}