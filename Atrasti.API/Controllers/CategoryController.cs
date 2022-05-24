using System.Collections.Generic;
using System.Threading.Tasks;
using Atrasti.API.Helpers;
using Atrasti.API.Models.Categories;
using Atrasti.Data.Core;
using Atrasti.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Atrasti.API.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IBaseCategoriesRepository _baseCategoriesRepository;

        public CategoryController(IBaseCategoriesRepository baseCategoriesRepository)
        {
            _baseCategoriesRepository = baseCategoriesRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> AllBaseCategories()
        {
            IList<BaseCategory> allCategories = await _baseCategoriesRepository.GetAllBaseCategories();
            return Ok(new BaseCategories_Res()
            {
                Categories = allCategories.MapCategoriesRes()
            });
        }
    }
}