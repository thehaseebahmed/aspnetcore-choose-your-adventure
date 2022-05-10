using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Tha.ChooseYourAdventure.Library.Constants;
using Tha.ChooseYourAdventure.Library.Exceptions;
using Tha.ChooseYourAdventure.Library.ViewModels;

namespace Tha.ChooseYourAdventure.WebAPI.Filters
{
    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(
            ILogger<GlobalExceptionFilter> logger
            )
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, context.Exception.Message);

            switch (context.Exception)
            {
                case NotFoundException ex:
                    context.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                    context.Result = new ObjectResult(
                        new ErrorViewModel { ErrorCode = ex.Code, ErrorMessage = ex.Message }
                        );
                    return;
                case ValidationException ex:
                    context.HttpContext.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                    context.Result = new ObjectResult(
                        new ValidationErrorsViewModel { ErrorMessages = ex.ErrorMessages }
                        );
                    return;
                default:
                    // DO NOTHING! DEFAULTS HAVE ALREADY BEEN SET AND
                    // THEY WOULD BE USED.
                    break;
            }

            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Result = new ObjectResult(
                new ErrorViewModel
                {
                    ErrorCode = ErrorConstants.UNKNOWN_ERROR_CODE,
                    ErrorMessage = ErrorConstants.UNKNOWN_ERROR_MESSAGE
                });
        }
    }
}
