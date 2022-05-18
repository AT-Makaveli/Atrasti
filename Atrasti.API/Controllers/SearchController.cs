using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Atrasti.API.Helpers;
using Atrasti.API.Models.Profile;
using Atrasti.API.Models.Search;
using Atrasti.Data.Core;
using Atrasti.Data.Models;
using Atrasti.Data.Models.Cache;
using Atrasti.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Atrasti.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class SearchController : Controller
    {
        private readonly SearchService _searchService;
        private readonly IUserRepository _userRepository;
        private readonly IUserCacheRepository _userCacheRepository;
        private readonly UserManager<AtrastiUser> _userManager;

        public SearchController(SearchService searchService, IUserRepository userRepository,
            UserManager<AtrastiUser> userManager, IUserCacheRepository userCacheRepository)
        {
            _searchService = searchService;
            _userRepository = userRepository;
            _userManager = userManager;
            _userCacheRepository = userCacheRepository;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Search([FromBody] Search_Req req)
        {
            AtrastiUser user = await _userManager.GetUserAsync(User);
            
            IReadOnlyCollection<SearchDocument> searchDocuments = await _searchService.Search(req.SearchQuery);

            IDictionary<int, SearchDocument> grouped = new Dictionary<int, SearchDocument>();

            foreach (SearchDocument searchDocument in searchDocuments)
            {
                if (!grouped.ContainsKey(searchDocument.CompanyId) && searchDocument.CompanyId != user.Id)
                    grouped.Add(searchDocument.CompanyId, searchDocument);
            }

            IEnumerable<int> companyIds = grouped.Values.Select(s => s.CompanyId).ToList();
            IEnumerable<AtrastiUser> users = await _userRepository.FindUsersByIds(companyIds);

            return Ok(new Search_Res()
            {
                Result = users.MapSearchEntries()
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CacheSearch([FromBody] CacheSearch_Req req)
        {
            AtrastiUser user = await _userManager.GetUserAsync(User);
            UserCache userCache = await _userCacheRepository.GetUserCache(user.Id, CacheType.SearchType);

            if (userCache == null) userCache = new UserCache()
            {
                RefId = user.Id,
                Type = CacheType.SearchType,
                Value = "[]"
            };

            IList<int> userIds = JsonConvert.DeserializeObject<IList<int>>(userCache.Value);
            if (userIds.Contains(req.CompanyId)) userIds.Remove(req.CompanyId);
            
            userIds.Add(req.CompanyId);

            userCache.Value = JsonConvert.SerializeObject(userIds);

            await _userCacheRepository.SaveSearchEntry(userCache);
            
            return Ok();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> SearchHistory()
        {
            AtrastiUser user = await _userManager.GetUserAsync(User);

            UserCache userCache = await _userCacheRepository.GetUserCache(user.Id, CacheType.SearchType);
            if (userCache == null)
            {
                return Ok(new Search_Res()
                {
                    Result = new List<SearchEntry_Res>()
                });
            }

            IList<int> userIds = JsonConvert.DeserializeObject<IList<int>>(userCache.Value);
            IList<AtrastiUser> users = await _userRepository.FindUsersByIds(userIds);
            IDictionary<int, AtrastiUser> usersDictionary = users.ToDictionary(x => x.Id, x => x);
            IList<AtrastiUser> ordered = new List<AtrastiUser>();
            
            foreach (int companyId in userIds)
            {
                if (usersDictionary.TryGetValue(companyId, out AtrastiUser cachedUser)) ordered.Add(cachedUser);
            }
            
            return Ok(new Search_Res()
            {
                Result = ordered.Reverse().MapSearchEntries()
            });
        }
    }
}