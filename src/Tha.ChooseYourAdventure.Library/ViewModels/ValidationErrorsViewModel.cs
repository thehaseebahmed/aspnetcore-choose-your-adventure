using System.Collections.Generic;
using Tha.ChooseYourAdventure.Library.Constants;

namespace Tha.ChooseYourAdventure.Library.ViewModels
{
    public class ValidationErrorsViewModel
    {
        public int ErrorCode { get; set; } = ErrorConstants.VALIDATION_ERROR_CODE;
        public List<ValidationErrorViewModel> ErrorMessages { get; set; } = new List<ValidationErrorViewModel>();
    }

    public class ValidationErrorViewModel
    {
        public string Field { get; set; }
        public string Message { get; set; }
    }
}
