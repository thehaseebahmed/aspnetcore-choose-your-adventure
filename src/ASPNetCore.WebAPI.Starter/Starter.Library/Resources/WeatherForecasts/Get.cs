using AutoMapper;
using System;

namespace Starter.Library.Resources.WeatherForecasts
{
    public class Get
    {
        public class Response
        {
            public DateTime Date { get; set; }

            public int TemperatureC { get; set; }

            public string Summary { get; set; }
        }

        public class Mapper : Profile
        {
            public Mapper()
            {
                CreateMap<Data.Entities.WeatherForecast, Response>();
            }
        }
    }
}
