using Microsoft.AspNetCore.Http;

namespace Atrasti.Models.Profile
{
    public class AccountSettingsModel
    {
        //Account settings
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string CompanyName { get; set; }

        public string CompanyImage { get; set; }

        //Profile settings
        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string Website { get; set; }

        public string CompanyDesc { get; set; }

        public string BusinessType { get; set; }

        public string MainProducts { get; set; }

        public string MainMarkets { get; set; }

        public int YearEstablished { get; set; }

        public string Certificates { get; set; }

        public string Capacity { get; set; }
    }
}
