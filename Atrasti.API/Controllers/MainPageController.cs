using System.Collections.Generic;
using System.Threading.Tasks;
using Atrasti.API.Helpers;
using Atrasti.API.Models.Home;
using Atrasti.Data.Core;
using Atrasti.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace Atrasti.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class MainPageController : Controller
    {
        private readonly IBaseCategoriesRepository _baseCategoriesRepository;

        public MainPageController(IBaseCategoriesRepository baseCategoriesRepository)
        {
            _baseCategoriesRepository = baseCategoriesRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            IList<BaseCategory> baseCategories = await _baseCategoriesRepository.GetAllBaseCategories();
            IList<HomeProducts_Res> productCategories = new List<HomeProducts_Res>();
            foreach (BaseCategory baseCategory in baseCategories)
            {
                IList<Product> products = await _baseCategoriesRepository.FindProductsByCategory(baseCategory);
                productCategories.Add(baseCategory.MapHomeProducts(products));
            }
            
            return Ok(new Home_Res()
            {
                Categories = productCategories
            });
        }
    }
}