using AutoMapper;
using MediatR;
using Starter.Data;
using Starter.Data.Interfaces;
using Starter.Library.ViewModels;
using System.Threading;
using System.Threading.Tasks;

namespace Starter.Library.Core
{
    public class Create
    {
        public class Handler<TCommand, TEntity, TKey> : IRequestHandler<TCommand, CommandResultViewModel>
            where TCommand : IRequest<CommandResultViewModel>
            where TEntity : class, IEntity<TKey>
        {
            private readonly ApiDbContext _db;
            private readonly IMapper _mapper;

            public Handler(
                ApiDbContext db,
                IMapper mapper
                )
            {
                _db = db;
                _mapper = mapper;
            }

            public async Task<CommandResultViewModel> Handle(
                TCommand request,
                CancellationToken cancellationToken
                )
            {
                var model = _mapper.Map<TEntity>(request);

                _db.Set<TEntity>().Add(model);
                await _db.SaveChangesAsync(cancellationToken);

                return _mapper.Map<CommandResultViewModel>(model);
            }
        }
    }
}
