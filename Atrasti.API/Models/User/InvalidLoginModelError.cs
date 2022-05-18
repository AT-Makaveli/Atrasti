using Atrasti.API.Models.Error;

namespace Atrasti.API.Models.User
{
    public class InvalidLoginModelError : BaseError
    {
        public const string AUTH_ALL_FIELDS = "AUTH_ALL_FIELDS";
        public const string INVALID_EMAIL_OR_PASSWORD = "INVALID_EMAIL_OR_PASSWORD";
        
        public InvalidLoginModelError(string errorTitle, string errorMessage)
        {
            Errors.Add(new ErrorEntry(errorTitle, errorMessage));
        }
    }
}