using System.Collections.Generic;
using Atrasti.API.Models.CountryInfo;

namespace Atrasti.API.Helpers
{
    public static class CountryHelpers
    {
        public static ICollection<InfoCountry_Res> MapCountries(this ICollection<CountryInfoModel> countryInfoModels)
        {
            ICollection<InfoCountry_Res> res = new List<InfoCountry_Res>();
            foreach (CountryInfoModel countryInfo in countryInfoModels)
            {
                res.Add(countryInfo.MapCountry());
            }

            return res;
        }

        public static InfoCountry_Res MapCountry(this CountryInfoModel countryInfoModel)
        {
            InfoCountry_Res res = new InfoCountry_Res();
            res.Country = countryInfoModel.Name;
            res.States = new Dictionary<string, InfoState_Res>();
            foreach (StateInfoModel state in countryInfoModel.States)
            {
                if (!res.States.ContainsKey(state.Name))
                    res.States.Add(state.Name, state.MapState());
            }

            return res;
        }

        public static InfoState_Res MapState(this StateInfoModel stateInfoModel)
        {
            InfoState_Res res = new InfoState_Res();
            res.State = stateInfoModel.Name;
            res.Cities = new Dictionary<string, InfoCity_Res>();
            foreach (CityInfoModel city in stateInfoModel.Cities)
            {
                if (!res.Cities.ContainsKey(city.Name))
                    res.Cities.Add(city.Name, city.MapCity());
            }

            return res;
        }

        public static InfoCity_Res MapCity(this CityInfoModel cityInfoModel)
        {
            InfoCity_Res res = new InfoCity_Res();
            res.CityName = cityInfoModel.Name;

            return res;
        }
    }
}