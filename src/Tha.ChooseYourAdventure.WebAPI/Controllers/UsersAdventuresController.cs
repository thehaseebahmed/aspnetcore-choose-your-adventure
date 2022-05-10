using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tha.ChooseYourAdventure.Library.ViewModels;
using System.Threading.Tasks;
using System;
using Tha.ChooseYourAdventure.Library.Resources.UserAdventures;
using Tha.ChooseYourAdventure.WebAPI.Filters;

namespace Tha.ChooseYourAdventure.WebAPI.Controllers
{
    [ApiController]
    [Route("/api/v1/users/{userId:Guid}/adventures")]
    [ProducesResponseType(typeof(ValidationErrorsViewModel), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ErrorViewModel), StatusCodes.Status500InternalServerError)]
    public class UsersAdventuresController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AdventuresController> _logger;

        public UsersAdventuresController(
            IMediator mediator,
            ILogger<AdventuresController> logger
            )
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResultViewModel<Get.Response>), StatusCodes.Status200OK)]
        [Transform]
        public async Task<IActionResult> Get(
            [FromRoute] Guid userId,
            [FromQuery] Get.Request query
            )
        {
            query.UserId = userId;
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("{userAdventureId:Guid}")]
        [ProducesResponseType(typeof(Get.Response), StatusCodes.Status200OK)]
        [Transform]
        public async Task<IActionResult> GetById(
            [FromRoute] Guid userAdventureId
            )
        {
            var query = new Library.Core.GetById.Request<Data.Entities.UserAdventure>() { Id = userAdventureId };
            var response = (await _mediator.Send(query));
            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CommandResultViewModel), StatusCodes.Status201Created)]
        public async Task<IActionResult> Post(
            [FromRoute] Guid userId,
            [FromBody] Create.Command command
            )
        {
            command.UserId = userId;
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPut("{userAdventureId:Guid}")]
        [ProducesResponseType(typeof(CommandResultViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put(
            [FromRoute] Guid userAdventureId,
            [FromBody] Put.Command command
            )
        {
            command.UserAdventureId = userAdventureId;
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
