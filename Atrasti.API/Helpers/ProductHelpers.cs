using Atrasti.API.Models.ProductModels;
using Atrasti.Data.Models;

namespace Atrasti.API.Helpers
{
    public static class ProductHelpers
    {
        public static Product_Res MapProductModel(this Product product)
        {
            return new Product_Res(product.Id, product.Title, product.Description, product.Tags, product.ProductLikes,
                product.IsHeartPressed);
        }
    }
}