using System.Collections.Generic;
using Atrasti.API.Models.ProductModels;
using Atrasti.Data.Models;

namespace Atrasti.API.Helpers
{
    public static class ProductHelpers
    {
        public static IList<Product_Res> MapProductsModel(this IList<Product> products)
        {
            IList<Product_Res> result = new List<Product_Res>();
            foreach (Product product in products)
            {
                result.Add(product.MapProductModel());
            }

            return result;
        }

        public static Product_Res MapProductModel(this Product product)
        {
            return new Product_Res(product.Id, product.Title, product.Description, product.Tags, product.ProductLikes,
                product.IsHeartPressed)
            {
                UserId = product.CompanyId
            };
        }
    }
}