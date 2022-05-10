using Microsoft.EntityFrameworkCore;
using System;
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
    }
}
