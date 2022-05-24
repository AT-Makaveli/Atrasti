using Atrasti.API.Models.Company;
using Atrasti.API.Models.Profile;
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

        public static CompanyInfo MapCompanyInfoReq(this CompanyInfo company, int refId, ManageProfile_Req profileReq)
        {
            if (company == null) company = new CompanyInfo();
            company.RefId = refId;
            company.BusinessType = profileReq.BusinessType;
            company.MainProducts = profileReq.MainProducts;
            company.MainMarkets = profileReq.MainMarkets;
            if (int.TryParse(profileReq.YearEstablished, out int year))
                company.YearEstablished = year;
            company.Certificates = profileReq.Certificates;
            company.Capacity = profileReq.Capacity;
            return company;
        }

        public static Company MapCompanyReq(this Company company, int refId, ManageProfile_Req profileReq)
        {
            if (company == null) company = new Company();
            company.RefId = refId;
            company.Address = profileReq.Address;
            company.City = profileReq.City;
            company.State = profileReq.County;
            company.Country = profileReq.Country;
            company.Website = profileReq.Website;
            company.CompanyDesc = profileReq.CompanyDesc;
            return company;
        }
    }
}