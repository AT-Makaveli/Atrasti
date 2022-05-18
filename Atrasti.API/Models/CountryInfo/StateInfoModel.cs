using System.Collections.Generic;

namespace Atrasti.API.Models.CountryInfo
{
    public class StateInfoModel
    {
        public string Name { get; set; }
        public ICollection<CityInfoModel> Cities { get; set; }
    }
}