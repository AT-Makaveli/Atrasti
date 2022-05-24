using Atrasti.API.Models.Company;
using Atrasti.Data.Models;

namespace Atrasti.API.Helpers
{
    public static class CompanyInfoHelpers
    {
        public static CompanyInfo_Res MapCompanyInfo(this CompanyInfo companyInfo)
        {
            return new CompanyInfo_Res()
            {
                BusinessType = companyInfo.BusinessType,
                Capacity = companyInfo.Capacity,
                Certificates = companyInfo.Certificates,
                MainMarkets = companyInfo.MainMarkets,
                MainProducts = companyInfo.MainProducts,
                YearEstablished = companyInfo.YearEstablished.ToString()
            };
        }
    }
}