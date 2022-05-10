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

                var lastStep = userAdventure.Steps.LastOrDefault()?.AdventureStep;
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
            private readonly IMapper _mapper;
            private readonly IRepository<UserAdventure> _userAdventureRepo;
            private readonly IRepository<UserAdventureStep> _userAdventureStepRepo;

            public Handler(
                IMapper mapper,
                IRepository<UserAdventure> userAdventureRepo,
                IRepository<UserAdventureStep> userAdventureStepRepo
                )
            {
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
                await _userAdventureStepRepo.CreateAsync(model);

                // 2a. CHECK IF THERE ARE ANY FURTHER STEPS OR IF THIS IS THE END
                var adventure = _userAdventureRepo.Read(request.AdventureStepId);
                if (!adventure.Adventure.Children.Any())
                {
                    // 2b. IF END, MARK THE USER'S ADVENTURE AS COMPLETED
                    var userAdventure = _userAdventureRepo.Read(request.UserAdventureId);
                    userAdventure.Status = UserAdventureStatus.Completed;

                    await _userAdventureRepo.UpdateAsync(request.UserAdventureId, userAdventure);
                }

                return _mapper.Map<CommandResultViewModel>(model);
            }
        }
    }
}
