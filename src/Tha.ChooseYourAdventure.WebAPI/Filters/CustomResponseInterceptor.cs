using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Tha.ChooseYourAdventure.WebAPI.Filters
{
    /// <summary>
    ///     At the time of writing this interceptor, FluentValidations does not support custom
    ///     validation responses (https://github.com/FluentValidation/FluentValidation/issues/548).
    ///     This interceptor allows to return a custom validation response by throwing an Exception
    ///     if !result.IsValid, which is then caught by the GlobalExceptionFilter and transformed
    ///     into the appropriate response.
    /// </summary>
    public class CustomResponseInterceptor : IValidatorInterceptor
    {
        public ValidationResult AfterMvcValidation(
            ControllerContext controllerContext,
            ValidationContext validationContext,
            ValidationResult result
            )
        {
            if (!result.IsValid)
            {
                throw new Library.Exceptions.ValidationException(result.Errors);
            }

            return result;
        }

        public ValidationContext BeforeMvcValidation(
            ControllerContext controllerContext,
            ValidationContext validationContext
            )
        {
            return validationContext;
        }
    }
}
