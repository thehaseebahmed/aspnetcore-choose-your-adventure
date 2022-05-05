﻿using AutoMapper;
using FluentValidation;
using MediatR;
using Tha.ChooseYourAdventure.Data.Entities;
using System;
using Tha.ChooseYourAdventure.Library.ViewModels;

namespace Tha.ChooseYourAdventure.Library.Resources.WeatherForecasts
{
    public class Create
    {
        public class Command : IRequest<CommandResultViewModel>
        {
            public DateTime Date { get; set; }
            public string Summary { get; set; }
            public int TemperatureC { get; set; }
        }

        public class Mapper : Profile
        {
            public Mapper()
            {
                CreateMap<Command, WeatherForecast>();
                CreateMap<WeatherForecast, CommandResultViewModel>();
            }
        }

        public class Validation : AbstractValidator<Command>
        {
            public Validation()
            {
                RuleFor(x => x.Date).NotEmpty();
                RuleFor(x => x.Summary).NotEmpty();
                RuleFor(x => x.TemperatureC).NotEmpty();
            }
        }
    }
}