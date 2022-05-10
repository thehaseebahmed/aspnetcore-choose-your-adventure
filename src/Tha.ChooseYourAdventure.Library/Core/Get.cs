using MediatR;
using Tha.ChooseYourAdventure.Data;
using Tha.ChooseYourAdventure.Library.Extensions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tha.ChooseYourAdventure.Library.Constants;
using Tha.ChooseYourAdventure.Library.ViewModels;
using Tha.ChooseYourAdventure.Library.Repositories;
using Tha.ChooseYourAdventure.Data.Interfaces;

namespace Tha.ChooseYourAdventure.Library.Core
{
    public class Get
    {
        public class Request<TEntity> : IPagedGetRequest, IRequest<PagedResultViewModel<TEntity>>
            where TEntity : class
        {
            public bool Count { get; set; } = InfraConstants.DEFAULT_QUERY_COUNT;
            public string Filter { get; set; }
            public int Limit { get; set; } = InfraConstants.DEFAULT_QUERY_LIMIT;
            public string Order { get; set; }
            public int Skip { get; set; }
        }

        public class Handler<TEntity> : IRequestHandler<Request<TEntity>, PagedResultViewModel<TEntity>>
            where TEntity : class, IEntity
        {
            private readonly IRepository<TEntity> _repo;

            public Handler(
                IRepository<TEntity> repo
                )
            {
                _repo = repo;
            }

            public async Task<PagedResultViewModel<TEntity>> Handle(
                Request<TEntity> request,
                CancellationToken cancellationToken
                )
            {
                var query = _repo.Read();

                if (request.Filter.HasValue())
                {
                    query = query.Filter(request.Filter);
                }

                if (request.Order.HasValue())
                {
                    query = query.Order(request.Order);
                }

                query = query.Page(request, out int count);

                return new PagedResultViewModel<TEntity>
                {
                    Count = count,
                    Data = query.AsEnumerable()
                };
            }
        }
    }
}
