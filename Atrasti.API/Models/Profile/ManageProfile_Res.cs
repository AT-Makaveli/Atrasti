using System.Collections.Generic;
using Atrasti.API.Models.Categories;
using Atrasti.API.Models.Company;

namespace Atrasti.API.Models.Profile
{
    public class ManageProfile_Res
    {
        public Company_Res Company { get; set; }
        public CompanyInfo_Res CompanyInfo { get; set; }
        public IList<BaseCategory_Res> UsedCategories { get; set; }
        public IList<BaseCategory_Res> AllCategories { get; set; }
    }
}