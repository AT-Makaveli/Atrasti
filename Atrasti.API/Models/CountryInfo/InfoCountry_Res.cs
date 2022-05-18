using System.Collections.Generic;

namespace Atrasti.API.Models.CountryInfo
{
    public class InfoCountry_Res
    {
        public string Country { get; set; }
        public IDictionary<string, InfoState_Res> States { get; set; }
    }
}