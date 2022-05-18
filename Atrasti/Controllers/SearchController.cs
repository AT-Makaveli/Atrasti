using System.Collections.Generic;
using System.Threading.Tasks;
using Atrasti.Search;
using Atrasti.Utils;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace Atrasti.Controllers
{
    public class SearchController : Controller
    {
        private readonly SearchService _searchService;

        public SearchController(SearchService searchService)
        {
            _searchService = searchService;
        }

        public async Task<JsonResult> SearchProducts(string search)
        {
            if (search == null) return Json(new {result = "none"});

            IReadOnlyCollection<SearchDocument> foundProducts = await _searchService.SearchProducts("products", search);
            if (foundProducts.Count == 0)
            {
                return Json(new {result = "none"});
            }
            IDictionary<int, SearchAlgorithm.SearchResult> searchResults =
                new Dictionary<int, SearchAlgorithm.SearchResult>();
            
            foreach (SearchDocument product in foundProducts)
            {
                if (!searchResults.ContainsKey(product.CompanyId))
                {
                    searchResults.Add(product.CompanyId, new SearchAlgorithm.SearchResult()
                    {
                        Title = product.Title
                    });
                }
            }
            
            return Json(new {result = "found", entities = searchResults.Values});
        }
    }
}