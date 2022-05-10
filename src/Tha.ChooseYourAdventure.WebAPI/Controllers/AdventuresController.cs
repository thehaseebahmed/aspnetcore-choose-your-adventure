using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tha.ChooseYourAdventure.Library.ViewModels;
using System.Threading.Tasks;
using System;
using Tha.ChooseYourAdventure.Library.Resources.Adventures;
using Tha.ChooseYourAdventure.WebAPI.Filters;

namespace Tha.ChooseYourAdventure.WebAPI.Controllers
{
    [ApiController]
    [Route("/api/v1/adventures")]
    [ProducesResponseType(typeof(ValidationErrorsViewModel), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ErrorViewModel), StatusCodes.Status500InternalServerError)]
    public class AdventuresController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AdventuresController> _logger;

        public AdventuresController(
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
            [FromQuery] Get.Request query
            )
        {
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("{id:Guid}")]
        [ProducesResponseType(typeof(GetById.Response), StatusCodes.Status200OK)]
        [Transform]
        public async Task<IActionResult> GetById(
            [FromRoute] Guid id
            )
        {
            var query = new GetById.Request() { Id = id };
            var response = (await _mediator.Send(query));
            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CommandResultViewModel), StatusCodes.Status201Created)]
        public async Task<IActionResult> Post(
            [FromBody] Create.Command command
            )
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
