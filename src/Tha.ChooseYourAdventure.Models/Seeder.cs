using Tha.ChooseYourAdventure.Data.Entities;
using System;
using System.Linq;

namespace Tha.ChooseYourAdventure.Data
{
    public static class Seeder
    {
        public static void Seed(ApiDbContext context)
        {
            string[] summaries = new[]
            {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };

            var rng = new Random();
            var randomData = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = summaries[rng.Next(summaries.Length)]
            });

            context.WeatherForecasts.AddRange(randomData);
            context.SaveChanges();
        }
    }
}
