using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using Tha.ChooseYourAdventure.Library.ViewModels;

namespace Tha.ChooseYourAdventure.Library.Exceptions
{
    public class ValidationException : Exception
    {
        public List<ValidationErrorViewModel> ErrorMessages { get; set; } = new List<ValidationErrorViewModel>();

        public ValidationException(
            string message,
            IList<ValidationFailure> errors
            ) : base(message)
        {
            ErrorMessages = errors
                .Select(e => new ValidationErrorViewModel { Field = e.PropertyName, Message = e.ErrorMessage })
                .ToList();
        }

        public ValidationException(
            IList<ValidationFailure> errors
            )
        {
            ErrorMessages = errors
                .Select(e => new ValidationErrorViewModel { Field = e.PropertyName, Message = e.ErrorMessage })
                .ToList();
        }
    }
}
