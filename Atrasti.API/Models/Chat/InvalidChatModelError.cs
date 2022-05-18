using Atrasti.API.Models.Error;

namespace Atrasti.API.Models.Chat
{
    public class InvalidChatModelError : BaseError
    {
        public const string USER_NOT_SET = "USER_NOT_SET";
        public const string USER_PROFILE_NOT_SET = "USER_PROFILE_NOT_SET";
        
        public InvalidChatModelError(string errorTitle, string errorMessage)
        {
            Errors.Add(new ErrorEntry(errorTitle, errorMessage));
        }
    }
}