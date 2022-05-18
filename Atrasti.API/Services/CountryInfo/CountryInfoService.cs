using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Atrasti.API.Models.CountryInfo;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace Atrasti.API.Services
{
    internal class CountryInfoService : ICountryInfoService
    {
        private readonly IHostEnvironment _hostingEnvironment;

        private static ICollection<CountryInfoModel> _loaded;
        private static int _gets = 0;

        public CountryInfoService(IHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<ICollection<CountryInfoModel>> GetCountries()
        {
            if (_loaded != null && _gets < 100)
            {
                _gets++;
                return _loaded;
            }

            await LoadJson();

            return _loaded;
        }

        public async Task LoadJson()
        {
            var rootPath = _hostingEnvironment.ContentRootPath;
            var fullPath =
                Path.Combine(rootPath,
                    "json/CountriesStatesCities.json");
            var jsonData = await File.ReadAllTextAsync(fullPath);
            ICollection<CountryInfoModel> countryInfoModel = JsonConvert.DeserializeObject<ICollection<CountryInfoModel>>(jsonData);
            _loaded = countryInfoModel;
            _gets = 0;
        }
    }
}