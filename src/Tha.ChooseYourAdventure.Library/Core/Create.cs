using AutoMapper;
using MediatR;
using Tha.ChooseYourAdventure.Data;
using Tha.ChooseYourAdventure.Data.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Tha.ChooseYourAdventure.Library.ViewModels;
using Tha.ChooseYourAdventure.Library.Repositories;

namespace Tha.ChooseYourAdventure.Library.Core
{
    public class Create
    {
        public class Handler<TCommand, TEntity> : IRequestHandler<TCommand, CommandResultViewModel>
            where TCommand : IRequest<CommandResultViewModel>
            where TEntity : class, IEntity
        {
            private readonly IMapper _mapper;
            private readonly IRepository<TEntity> _repo;

            public Handler(
                IMapper mapper,
                IRepository<TEntity> repo
                )
            {
                _repo = repo;
                _mapper = mapper;
            }

            public async Task<CommandResultViewModel> Handle(
                TCommand request,
                CancellationToken cancellationToken
                )
            {
                var model = _mapper.Map<TEntity>(request);
                model = await _repo.CreateAsync(model, cancellationToken);

                return _mapper.Map<CommandResultViewModel>(model);
            }
        }
    }
}
