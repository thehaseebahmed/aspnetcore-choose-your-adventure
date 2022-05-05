using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tha.ChooseYourAdventure.Library.Resources.WeatherForecasts;
using Tha.ChooseYourAdventure.Library.ViewModels;
using Tha.ChooseYourAdventure.WebAPI.Filters;
using System.Threading.Tasks;

namespace Tha.ChooseYourAdventure.WebAPI.Controllers
{
    [ApiController]
    [Route("/api/v1/weather-forecast")]
    [ProducesResponseType(typeof(ValidationErrorsViewModel), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ErrorViewModel), StatusCodes.Status500InternalServerError)]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(
            IMediator mediator,
            ILogger<WeatherForecastController> logger
            )
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResultViewModel<Get.Response>), StatusCodes.Status200OK)]
        [Transform]
        public async Task<IActionResult> Get(
            [FromQuery] Library.Core.Get.Request<Data.Entities.WeatherForecast> query
            )
        {
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CommandResultViewModel), StatusCodes.Status201Created)]
        public async Task<IActionResult> Post(
            [FromQuery] Create.Command command
            )
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
