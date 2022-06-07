using System.Collections.Generic;
using Atrasti.API.Models.Company;
using Atrasti.API.Models.ProductModels;

namespace Atrasti.API.Models.Profile
{
    public class CompanyPage_Res
    {
        public ICollection<Product_Res> Products { get; set; } 
        public Company_Res Company { get; set; }
        public CompanyInfo_Res CompanyInfo { get; set; }
    }
}