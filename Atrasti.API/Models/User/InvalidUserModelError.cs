using Atrasti.API.Models.Error;

namespace Atrasti.API.Models.User
{
    public class InvalidUserModelError : BaseError
    {
        public const string FCM_TOKEN_NOT_SET = "FCM_TOKEN_NOT_SET";
        
        public InvalidUserModelError(string errorTitle, string errorMessage)
        {
            Errors.Add(new ErrorEntry(errorTitle, errorMessage));
        }
    }
}