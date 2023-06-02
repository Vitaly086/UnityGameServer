using Microsoft.EntityFrameworkCore;
using Server.Models;
using Server.Models.Inventory;

namespace Server
{
    public class GameDbContext : DbContext
    {
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<HeroesSettings> HeroesSettings { get; set; }
        public DbSet<DefaultHeroes> DefaultHeroes { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<UserInventory> UserInventories { get; set; }

        public GameDbContext(DbContextOptions<GameDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<HeroesSettings>(entity =>
            {
                entity.HasOne(h => h.UserProfile)
                    .WithMany(u => u.HeroesSettings)
                    .HasForeignKey(h => h.UserId);
            });

            modelBuilder.Entity<UserInventory>(entity =>
            {
                entity.HasOne(ui => ui.User)
                    .WithMany(u => u.Inventory)
                    .HasForeignKey(ui => ui.UserId);
            });
        }
    }
}