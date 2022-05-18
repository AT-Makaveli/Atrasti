using Atrasti.API.Models.Error;

namespace Atrasti.API.Models.User
{
    public class InvalidRegisterModelError : BaseError
    {
        public const string COMPANY_EMPTY = "COMPANY_EMPTY";
        public const string COMPANY_IN_USE = "COMPANY_IN_USE";
        
        public const string EMAIL_EMPTY = "EMAIL_EMPTY";
        public const string EMAIL_INVALID = "EMAIL_INVALID";
        public const string EMAIL_IN_USE = "EMAIL_IN_USE";
        
        public const string FIRST_NAME_EMPTY = "FIRST_NAME_EMPTY";
        public const string LAST_NAME_EMPTY = "LAST_NAME_EMPTY";
        
        public const string PASSWORD_INVALID = "PASSWORD_INVALID";

        public const string UNKNOWN_ERROR = "UNKNOWN_ERROR";
        
        public InvalidRegisterModelError(string errorTitle, string errorMessage)
        {
            Errors.Add(new ErrorEntry(errorTitle, errorMessage));
        }
    }
}