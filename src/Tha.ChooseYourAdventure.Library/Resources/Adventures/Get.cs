using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tha.ChooseYourAdventure.Data;
using Tha.ChooseYourAdventure.Data.Entities;
using Tha.ChooseYourAdventure.Library.Constants;
using Tha.ChooseYourAdventure.Library.Core;
using Tha.ChooseYourAdventure.Library.Extensions;
using Tha.ChooseYourAdventure.Library.Repositories;
using Tha.ChooseYourAdventure.Library.ViewModels;

namespace Tha.ChooseYourAdventure.Library.Resources.Adventures
{
    public class Get
    {
        public class Request : IPagedGetRequest, IRequest<PagedResultViewModel<AdventureNode>>
        {
            public bool Count { get; set; } = InfraConstants.DEFAULT_QUERY_COUNT;
            public int Limit { get; set; } = InfraConstants.DEFAULT_QUERY_LIMIT;
            public int Skip { get; set; }
        }

        public class Response
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }

        public class Mapper : Profile
        {
            public Mapper()
            {
                CreateMap<AdventureNode, Response>();
            }
        }

        public class Handler : IRequestHandler<Request, PagedResultViewModel<AdventureNode>>
        {
            private readonly IRepository<AdventureNode> _repo;

            public Handler(
                IRepository<AdventureNode> repo
                )
            {
                _repo = repo;
            }

            public async Task<PagedResultViewModel<AdventureNode>> Handle(
                Request request,
                CancellationToken cancellationToken
                )
            {
                var query = _repo.Read()
                    .Where(a => a.IsRootNode)
                    .Page(request, out int count)
                    .AsNoTrackingWithIdentityResolution();

                return new PagedResultViewModel<AdventureNode>
                {
                    Count = count,
                    Data = query.ToList()
                };
            }
        }
    }
}
