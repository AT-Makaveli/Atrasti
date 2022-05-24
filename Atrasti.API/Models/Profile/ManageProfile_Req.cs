using System.Collections.Generic;
using Atrasti.API.Models.Categories;

namespace Atrasti.API.Models.Profile
{
    public class ManageProfile_Req
    {
        public string Logo { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string Country { get; set; }
        public string Website { get; set; }
        public string CompanyDesc { get; set; }
        public string PhoneNumber { get; set; }
        public string BusinessType { get; set; }
        public string MainProducts { get; set; }
        public string MainMarkets { get; set; }
        public string Certificates { get; set; }
        public string YearEstablished { get; set; }
        public string Capacity { get; set; }
        public IList<BaseCategory_Res> NewCategories { get; set; }
    }
}