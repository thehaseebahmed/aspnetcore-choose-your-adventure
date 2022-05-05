using FluentValidation.Results;
using Starter.Library.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Starter.Library.Exceptions
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
