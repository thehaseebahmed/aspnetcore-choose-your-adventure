using MediatR;
using Tha.ChooseYourAdventure.Data;
using System.Threading;
using System.Threading.Tasks;
using Tha.ChooseYourAdventure.Library.Exceptions;
using Tha.ChooseYourAdventure.Library.Constants;
using Tha.ChooseYourAdventure.Data.Interfaces;
using Tha.ChooseYourAdventure.Library.Repositories;
using System.Linq;

namespace Tha.ChooseYourAdventure.Library.Core
{
    public class GetById
    {
        public class Request<TEntity> : IRequest<TEntity>
            where TEntity : class, IEntity
        {
            public object Id { get; set; }
        }

        public class Handler<TEntity> : IRequestHandler<Request<TEntity>, TEntity>
            where TEntity : class, IEntity
        {
            private readonly IRepository<TEntity> _repo;

            public Handler(
                IRepository<TEntity> repo
                )
            {
                _repo = repo;
            }

            public async Task<TEntity> Handle(
                Request<TEntity> request,
                CancellationToken cancellationToken
                )
            {
                var entity = _repo.Read().FirstOrDefault(e => e.Id.Equals(request.Id));
                if (entity == null)
                {
                    throw new NotFoundException(ErrorConstants.NOT_FOUND_ERROR_MESSAGE);
                }

                return entity;
            }
        }
    }
}
