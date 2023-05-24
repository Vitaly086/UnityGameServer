using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Server.Models;

namespace Server.Services;

public class HeroSettingsService
{
    private readonly GameDbContext _context;

    private readonly string _defaultHeroSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "Models", "DefaultHeroSettings.json");

    public HeroSettingsService(GameDbContext context)
    {
        _context = context;
    }

    public void CreateDefaultHeroSettings(int userId)
    {
        var jsonString = File.ReadAllText(_defaultHeroSettingsPath);

        var heroSettings = JsonConvert.DeserializeObject<HeroSettings>(jsonString);

        heroSettings.UserId = userId;
        _context.HeroSettings.AddRange(heroSettings);
        _context.SaveChanges();
    }

    public List<HeroSettings> LoadHeroesSettings(int userId)
    {
        return _context.HeroSettings.Include(hero => hero.UserProfile)
            .Where(hero => hero.UserProfile.Id == userId)
            .ToList();
    }
}