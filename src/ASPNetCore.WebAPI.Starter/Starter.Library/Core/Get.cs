using FluentValidation;
using MediatR;
using Starter.Data;
using Starter.Library.Constants;
using Starter.Library.Extensions;
using Starter.Library.ViewModels;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Starter.Library.Core
{
    public class Get
    {
        public class Request<TEntity> : IRequest<PagedResultViewModel<TEntity>>
            where TEntity : class
        {
            public bool Count { get; set; } = InfraConstants.DEFAULT_QUERY_COUNT;
            public string Filter { get; set; }
            public int Limit { get; set; } = InfraConstants.DEFAULT_QUERY_LIMIT;
            public string Order { get; set; }
            public int Skip { get; set; }
        }

        public class Handler<TEntity> : IRequestHandler<Request<TEntity>, PagedResultViewModel<TEntity>>
            where TEntity : class
        {
            private readonly ApiDbContext _db;

            public Handler(
                ApiDbContext db
                )
            {
                _db = db;
            }

            public async Task<PagedResultViewModel<TEntity>> Handle(
                Request<TEntity> request,
                CancellationToken cancellationToken
                )
            {
                var query = _db.Set<TEntity>().AsQueryable();

                if (request.Filter.HasValue())
                {
                    query = query.Filter(request.Filter);
                }

                if (request.Order.HasValue())
                {
                    query = query.Order(request.Order);
                }

                if (request.Skip > 0)
                {
                    query = query.Skip(request.Skip);
                }

                if (request.Limit > 0)
                {
                    query = query.Take(request.Limit);
                }

                int count = 0;
                if (request.Count)
                {
                    count = query.Count();
                }

                var data = query.AsEnumerable();

                return new PagedResultViewModel<TEntity>
                {
                    Count = count,
                    Data = data
                };
            }
        }
    }
}
