using System.Collections.Generic;
using Atrasti.API.Models.Home;
using Atrasti.Data.Models;

namespace Atrasti.API.Helpers
{
    public static class HomeHelpers
    {
        public static HomeProducts_Res MapHomeProducts(this BaseCategory baseCategory, IList<Product> products)
        {
            return new HomeProducts_Res()
            {
                Category = baseCategory.Title,
                Products = products.MapProductsModel()
            };
        }
    }
}