using Microsoft.EntityFrameworkCore;
using Tha.ChooseYourAdventure.Data.Entities;

namespace Tha.ChooseYourAdventure.Data
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(
            DbContextOptions<ApiDbContext> options
            ) : base(options) { }

        public DbSet<WeatherForecast> WeatherForecasts { get; set; }
    }
}
