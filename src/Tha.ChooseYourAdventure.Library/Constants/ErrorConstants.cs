namespace Tha.ChooseYourAdventure.Library.Constants
{
    public class ErrorConstants
    {
        public const int UNKNOWN_ERROR_CODE = 1;
        public const string UNKNOWN_ERROR_MESSAGE = "Something went wrong! Please try again later.";

        public const int VALIDATION_ERROR_CODE = 20;
        public const string VALIDATION_ERROR_MESSAGE = "";

        public const string VALIDATION_ERROR_COMPLETED_ADVENTURE = "You've already completed this adventure! Please try another one.";
        public const string VALIDATION_ERROR_DUPLICATE_ADVENTURE = "You've already been on this adventure! Please try another one.";
        public const string VALIDATION_ERROR_INVALID_ADVENTURE_ID = "You've choosen an invalid adventure. Please choose the correct option & try again.";
        public const string VALIDATION_ERROR_INVALID_ADVENTURE_STEP = "You've choosen an invalid option. Please choose the correct option & try again.";

        public const int NOT_FOUND_ERROR_CODE = 40;
        public const string NOT_FOUND_ERROR_MESSAGE = "Requested resource does not exist or could not be found.";
    }
}
