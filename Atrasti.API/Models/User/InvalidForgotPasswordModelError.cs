using Atrasti.API.Models.Error;

namespace Atrasti.API.Models.User
{
    public class InvalidForgotPasswordModelError : BaseError
    {
        public const string EMAIL_EMPTY = "EMAIL_EMPTY";
        public const string EMAIL_INVALID = "EMAIL_INVALID";
        
        public InvalidForgotPasswordModelError(string errorTitle, string errorMessage)
        {
            Errors.Add(new ErrorEntry(errorTitle, errorMessage));
        }
    }
}