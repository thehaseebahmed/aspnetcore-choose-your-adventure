using System;
using Tha.ChooseYourAdventure.Data.Interfaces;

namespace Tha.ChooseYourAdventure.Data.Entities
{
    public class WeatherForecast : IEntity<Guid>
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public string Summary { get; set; }
    }
}
