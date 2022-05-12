using Microsoft.EntityFrameworkCore;
using Tha.ChooseYourAdventure.Data.Entities;

namespace Tha.ChooseYourAdventure.Data
{
    public class ApiDbContext : DbContext
    {
        public DbSet<AdventureNode> Adventures { get; set; }
        public DbSet<UserAdventure> UserAdventures { get; set; }
        public DbSet<UserAdventureStep> UserAdventureSteps { get; set; }

        public ApiDbContext(
            DbContextOptions<ApiDbContext> options
            ) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AdventureNode>()
                .HasMany(e => e.Children)
                .WithOne()
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AdventureNode>()
                .HasOne<AdventureNode>()
                .WithMany(e => e.Children)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserAdventure>()
                .HasMany(e => e.Steps)
                .WithOne()
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserAdventure>()
                .HasOne(e => e.Adventure)
                .WithMany()
                .IsRequired(true)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserAdventureStep>()
                .HasOne(e => e.AdventureStep)
                .WithOne()
                .IsRequired(true)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserAdventureStep>()
                .HasOne<UserAdventure>()
                .WithMany(e => e.Steps)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
