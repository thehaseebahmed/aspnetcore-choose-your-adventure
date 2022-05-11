using AutoMapper;
using FluentValidation;
using FluentValidation.Validators;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tha.ChooseYourAdventure.Data.Entities;
using Tha.ChooseYourAdventure.Data.Enums;
using Tha.ChooseYourAdventure.Library.Constants;
using Tha.ChooseYourAdventure.Library.Repositories;
using Tha.ChooseYourAdventure.Library.ViewModels;

namespace Tha.ChooseYourAdventure.Library.Resources.UserAdventures
{
    public class Put
    {
        public class Command : IRequest<CommandResultViewModel>
        {
            public Guid AdventureStepId { get; set; }
            public Guid UserAdventureId { get; set; }
            public Guid UserId { get; set; }
        }

        public class Mapper : Profile
        {
            public Mapper()
            {
                CreateMap<Command, UserAdventureStep>();
                CreateMap<UserAdventureStep, CommandResultViewModel>();
            }
        }

        public class Validation : AbstractValidator<Command>
        {
            private IRepository<UserAdventure> _repo { get; }

            public Validation(
                IRepository<UserAdventure> repo
                )
            {
                _repo = repo;

                RuleFor(e => e.AdventureStepId).NotEmpty();
                RuleFor(e => e.UserAdventureId).NotEmpty();

                RuleFor(e => e).Custom(HaveAValidUserAdventureIdAndAdventureId);
            }

            private void HaveAValidUserAdventureIdAndAdventureId(Command req, CustomContext ctx)
            {
                var userAdventure = _repo.Read()
                    .Include(ua => ua.Adventure)
                    .ThenInclude(ua => ua.Children)
                    .Include(ua => ua.Steps)
                    .ThenInclude(s => s.AdventureStep.Children)
                    .FirstOrDefault(ua => ua.Id.Equals(req.UserAdventureId));

                if (userAdventure == null)
                {
                    ctx.AddFailure(ErrorConstants.VALIDATION_ERROR_INVALID_ADVENTURE_ID);
                    return;
                }

                if (userAdventure.Status == UserAdventureStatus.Completed)
                {
                    ctx.AddFailure(ErrorConstants.VALIDATION_ERROR_COMPLETED_ADVENTURE);
                    return;
                }

                var lastStep = userAdventure.Steps
                    .OrderBy(s => s.CreatedOn)
                    .LastOrDefault()?
                    .AdventureStep;

                if (lastStep == null && userAdventure.Adventure.Children.Any(c => c.Id.Equals(req.AdventureStepId)))
                {
                    return;
                }

                if (lastStep != null && lastStep.Children.Any(c => c.Id.Equals(req.AdventureStepId)))
                {
                    return;
                }

                ctx.AddFailure(ErrorConstants.VALIDATION_ERROR_INVALID_ADVENTURE_STEP);
            }
        }

        public class Handler : IRequestHandler<Command, CommandResultViewModel>
        {
            private readonly IRepository<AdventureNode> _adventureRepo;
            private readonly IMapper _mapper;
            private readonly IRepository<UserAdventure> _userAdventureRepo;
            private readonly IRepository<UserAdventureStep> _userAdventureStepRepo;

            public Handler(
                IRepository<AdventureNode> adventureRepo,
                IMapper mapper,
                IRepository<UserAdventure> userAdventureRepo,
                IRepository<UserAdventureStep> userAdventureStepRepo
                )
            {
                _adventureRepo = adventureRepo;
                _mapper = mapper;
                _userAdventureRepo = userAdventureRepo;
                _userAdventureStepRepo = userAdventureStepRepo;
            }

            public async Task<CommandResultViewModel> Handle(
                Command request,
                CancellationToken cancellationToken
                )
            {
                // 1. CREATE A NEW STEP IN USER'S ADVENTURE
                var model = _mapper.Map<UserAdventureStep>(request);
                model = await _userAdventureStepRepo.CreateAsync(model);

                // 2a. CHECK IF THERE ARE ANY FURTHER STEPS OR IF THIS IS THE END
                var adventure = _adventureRepo.Read()
                    .Include(e => e.Children)
                    .FirstOrDefault(e => e.Id.Equals(request.AdventureStepId));

                if (!adventure.Children.Any())
                {
                    // 2b. IF END, MARK THE USER'S ADVENTURE AS COMPLETED
                    var userAdventure = _userAdventureRepo.Read().FirstOrDefault(e => e.Id.Equals(request.UserAdventureId));
                    userAdventure.Status = UserAdventureStatus.Completed;

                    await _userAdventureRepo.UpdateAsync(request.UserAdventureId, userAdventure);
                }

                return _mapper.Map<CommandResultViewModel>(model);
            }
        }
    }
}
