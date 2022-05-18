using Atrasti.API.Models.Company;
using Atrasti.Data.Models;

namespace Atrasti.API.Helpers
{
    public static class CompanyHelpers
    {
        public static Company_Res MapCompany(this Company company)
        {
            return new Company_Res()
            {
                Address = company.Address,
                City = company.City,
                County = company.State,
                CompanyDesc = company.CompanyDesc,
                Country = company.Country,
                Website = company.Website
            };
        }
    }
}