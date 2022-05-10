using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using Tha.ChooseYourAdventure.Library.Constants;
using Tha.ChooseYourAdventure.Library.ViewModels;

namespace Tha.ChooseYourAdventure.Library.Exceptions
{
    public class NotFoundException : Exception
    {
        public readonly int Code = ErrorConstants.NOT_FOUND_ERROR_CODE;

        public NotFoundException()
            : base(ErrorConstants.NOT_FOUND_ERROR_MESSAGE) { }

        public NotFoundException(string message)
            : base(message) { }
    }
}
