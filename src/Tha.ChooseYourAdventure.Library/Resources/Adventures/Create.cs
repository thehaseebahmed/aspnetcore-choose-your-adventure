using AutoMapper;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using Tha.ChooseYourAdventure.Data.Entities;
using Tha.ChooseYourAdventure.Library.ViewModels;

namespace Tha.ChooseYourAdventure.Library.Resources.Adventures
{
    public class Create
    {
        public class Command : IRequest<CommandResultViewModel>
        {
            public Command()
            {
                Children = new List<Command>();
                IsRootNode = true;
            }

            public ICollection<Command> Children { get; set; }
            public bool IsRootNode { get; set; }
            public string Name { get; set; }
            public string OptionTitle { get; set; }
        }

        public class Mapper : Profile
        {
            public Mapper()
            {
                CreateMap<Command, AdventureNode>();
                CreateMap<AdventureNode, CommandResultViewModel>();
            }
        }

        public class Validation : AbstractValidator<Command>
        {
            public Validation()
            {

            }
        }
    }
}
