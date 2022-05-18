using Atrasti.API.Models.Error;

namespace Atrasti.API.Models.Profile
{
    public class InvalidProfileModelError : BaseError
    {
        public const string USER_NOT_SET = "USER_NOT_SET";
        public const string PRODUCT_NOT_SET = "PRODUCT_NOT_SET";
        
        public InvalidProfileModelError(string errorTitle, string errorMessage)
        {
            Errors.Add(new ErrorEntry(errorTitle, errorMessage));
        }
    }
}