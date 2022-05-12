using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tha.ChooseYourAdventure.Data.Entities;
using Tha.ChooseYourAdventure.Data.Enums;
using Tha.ChooseYourAdventure.Library.Constants;
using Tha.ChooseYourAdventure.Library.Core;
using Tha.ChooseYourAdventure.Library.Extensions;
using Tha.ChooseYourAdventure.Library.Repositories;
using Tha.ChooseYourAdventure.Library.ViewModels;

namespace Tha.ChooseYourAdventure.Library.Resources.UserAdventures
{
    public class Get
    {
        public class Request : IPagedGetRequest, IRequest<PagedResultViewModel<UserAdventure>>
        {
            public bool Count { get; set; } = InfraConstants.DEFAULT_QUERY_COUNT;
            public int Limit { get; set; } = InfraConstants.DEFAULT_QUERY_LIMIT;
            public int Skip { get; set; }
            public Guid UserId { get; set; }
        }

        public class Response
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string OptionTitle { get; set; }
            public UserAdventureStatus Status { get; set; }
            public ICollection<ResponseSteps> Steps { get; set; }

            public class ResponseSteps
            {
                public Guid Id { get; set; }
                public string Name { get; set; }
                public string OptionTitle { get; set; }
            }
        }

        public class Mapper : Profile
        {
            public Mapper()
            {
                CreateMap<UserAdventure, Response>()
                    .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Adventure.Name))
                    .ForMember(dst => dst.OptionTitle, opt => opt.MapFrom(src => src.Adventure.OptionTitle))
                    ;

                CreateMap<UserAdventureStep, Response.ResponseSteps>()
                    .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.AdventureStep.Name))
                    .ForMember(dst => dst.OptionTitle, opt => opt.MapFrom(src => src.AdventureStep.OptionTitle))
                    ;
            }
        }

        public class Validation : AbstractValidator<Request>
        {
            public Validation()
            {
                RuleFor(r => r.UserId).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Request, PagedResultViewModel<UserAdventure>>
        {
            private readonly IRepository<UserAdventure> _repo;

            public Handler(
                IRepository<UserAdventure> repo
                )
            {
                _repo = repo;
            }

            public async Task<PagedResultViewModel<UserAdventure>> Handle(
                Request request,
                CancellationToken cancellationToken
                )
            {
                var query = _repo.Read()
                    .Include(ua => ua.Adventure)
                    .Include(ua => ua.Steps)
                    .ThenInclude(s => s.AdventureStep)
                    .Where(ua => ua.UserId == request.UserId)
                    .Page(request, out int count)
                    .AsNoTrackingWithIdentityResolution();

                return new PagedResultViewModel<UserAdventure>
                {
                    Count = count,
                    Data = query.AsEnumerable()
                };
            }
        }
    }
}
