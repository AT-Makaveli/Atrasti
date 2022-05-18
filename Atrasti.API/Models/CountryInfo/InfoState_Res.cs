using System.Collections.Generic;

namespace Atrasti.API.Models.CountryInfo
{
    public class InfoState_Res
    {
        public string State { get; set; }
        public IDictionary<string, InfoCity_Res> Cities { get; set; }
    }
}