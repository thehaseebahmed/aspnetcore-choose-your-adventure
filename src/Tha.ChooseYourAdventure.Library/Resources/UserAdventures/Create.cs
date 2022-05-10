using AutoMapper;
using FluentValidation;
using MediatR;
using System;
using Tha.ChooseYourAdventure.Data.Entities;
using Tha.ChooseYourAdventure.Library.ViewModels;

namespace Tha.ChooseYourAdventure.Library.Resources.UserAdventures
{
    public class Create
    {
        public class Command : IRequest<CommandResultViewModel>
        {
            public Guid AdventureId { get; set; }
            public Guid UserId { get; set; }
        }

        public class Mapper : Profile
        {
            public Mapper()
            {
                CreateMap<Command, UserAdventure>();
                CreateMap<UserAdventure, CommandResultViewModel>();
            }
        }

        public class Validation : AbstractValidator<Command>
        {
            public Validation()
            {
                RuleFor(e => e.AdventureId).NotEmpty();
                RuleFor(e => e.UserId).NotEmpty();
            }
        }
    }
}
