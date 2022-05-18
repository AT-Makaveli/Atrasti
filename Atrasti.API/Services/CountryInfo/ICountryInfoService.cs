using System.Collections.Generic;
using System.Threading.Tasks;
using Atrasti.API.Models.CountryInfo;

namespace Atrasti.API.Services
{
    public interface ICountryInfoService
    {
        Task<ICollection<CountryInfoModel>> GetCountries();
        
        Task LoadJson();
    }
}