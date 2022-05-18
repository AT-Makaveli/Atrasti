using System.Collections.Generic;

namespace Atrasti.API.Models.CountryInfo
{
    public class CountryInfoModel
    {
        public string Name { get; set; }
        public ICollection<StateInfoModel> States { get; set; }
    }
}