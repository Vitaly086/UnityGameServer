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
        public DbSet<InventoryItem> InventoryItems { get; set; }
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
                entity.HasKey(ui => new { ui.UserId, ui.ItemId });

                entity.HasOne(ui => ui.User)
                    .WithMany(u => u.Inventory)
                    .HasForeignKey(ui => ui.UserId);

                entity.HasOne(ui => ui.Item)
                    .WithMany(i => i.UserInventories)
                    .HasForeignKey(ui => ui.ItemId);
            });

            modelBuilder.Entity<InventoryItemType>(entity =>
            {
                entity.HasKey(ii => ii.Id);

                entity.HasOne(ii => ii.InventoryItem)
                    .WithMany(i => i.InventoryItemTypes)
                    .HasForeignKey(ii => ii.InventoryItemId);
            });
        }
    }
}