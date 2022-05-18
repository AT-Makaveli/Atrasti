using System.Collections.Generic;
using System.Threading.Tasks;
using Atrasti.API.Helpers;
using Atrasti.API.Models.CountryInfo;
using Atrasti.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Atrasti.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CountryController : Controller
    {
        private readonly ICountryInfoService _countryInfoService;

        public CountryController(ICountryInfoService countryInfoService)
        {
            _countryInfoService = countryInfoService;
        }

        [HttpGet]
        public async Task<IActionResult> Countries()
        {
            ICollection<CountryInfoModel> countries = await _countryInfoService.GetCountries();
            ICollection<InfoCountry_Res> res = countries.MapCountries();
            return Ok(res);
        }
    }
}