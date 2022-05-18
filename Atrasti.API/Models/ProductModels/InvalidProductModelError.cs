using Atrasti.API.Models.Error;

namespace Atrasti.API.Models.ProductModels
{
    public class InvalidProductModelError : BaseError
    {
        public const string PRODUCT_IMAGE_NOT_SET = "IMAGE_NOT_SET";
        public const string PRODUCT_TITLE_NOT_SET = "PRODUCT_TITLE_NOT_SET";
        public const string PRODUCT_DESC_NOT_SET = "PRODUCT_DESC_NOT_SET";
        public const string PRODUCT_TAGS_NOT_SET = "PRODUCT_TAGS_NOT_SET";
        
        public InvalidProductModelError(string errorTitle, string errorMessage)
        {
            Errors.Add(new ErrorEntry(errorTitle, errorMessage));
        }
    }
}