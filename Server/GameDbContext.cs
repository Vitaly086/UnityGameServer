using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server;

public class GameDbContext : DbContext
{
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<Heroes> Heroes { get; set; }
    public DbSet<DefaultHeroes> DefaultHeroes  { get; set; }

    public GameDbContext(DbContextOptions options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Heroes>()
            .HasOne(h => h.UserProfile)
            .WithMany(u => u.Heroes)
            .HasForeignKey(h => h.UserId);
        
        base.OnModelCreating(modelBuilder);
    }
}