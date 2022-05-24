using System.Collections.Generic;
using Atrasti.API.Models.ProductModels;

namespace Atrasti.API.Models.Home
{
    public class HomeProducts_Res
    {
        public string Category { get; set; }
        public IList<Product_Res> Products { get; set; }
    }
}