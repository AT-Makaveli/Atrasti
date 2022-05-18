using System.Collections.Generic;
using Atrasti.API.Models.ProductModels;

namespace Atrasti.API.Models.Profile
{
    public class CompanyPage_Res
    {
        public ICollection<Product_Res> Products { get; set; } 
    }
}