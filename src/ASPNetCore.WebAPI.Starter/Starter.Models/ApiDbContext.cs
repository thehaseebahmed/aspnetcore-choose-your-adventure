using Microsoft.EntityFrameworkCore;
using Starter.Data.Entities;

namespace Starter.Data
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(
            DbContextOptions<ApiDbContext> options
            ) : base(options) { }

        public DbSet<WeatherForecast> WeatherForecasts { get; set; }
    }
}
