using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tha.ChooseYourAdventure.Data.Entities;
using Tha.ChooseYourAdventure.Library.Constants;
using Tha.ChooseYourAdventure.Library.Exceptions;
using Tha.ChooseYourAdventure.Library.Repositories;

namespace Tha.ChooseYourAdventure.Library.Resources.Adventures
{
    public class GetById
    {
        public class Request : IRequest<AdventureNode>
        {
            public Guid Id { get; set; }
        }

        public class Response
        {
            public Guid Id { get; set; }

            public Response()
            {
                Children = new List<Response>();
            }

            public IEnumerable<Response> Children { get; set; }
            public string Name { get; set; }
            public string OptionTitle { get; set; }
        }

        public class Mapper : Profile
        {
            public Mapper()
            {
                CreateMap<AdventureNode, Response>();
            }
        }

        public class Handler : IRequestHandler<Request, AdventureNode>
        {
            private readonly IRepository<AdventureNode> _repo;

            public Handler(
                IRepository<AdventureNode> repo
                )
            {
                _repo = repo;
            }

            public async Task<AdventureNode> Handle(
                Request request,
                CancellationToken cancellationToken
                )
            {
                var entity = await _repo.Read()
                    .Include(a => a.Children)
                    .FirstOrDefaultAsync(a => a.Id.Equals(request.Id));

                if (entity == null)
                {
                    throw new NotFoundException(ErrorConstants.NOT_FOUND_ERROR_MESSAGE);
                }

                return entity;
            }
        }
    }
}
