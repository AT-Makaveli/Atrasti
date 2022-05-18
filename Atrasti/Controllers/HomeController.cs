using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Atrasti.Models;
using System.Threading.Tasks;
using Atrasti.Data.Core;
using Microsoft.AspNetCore.Identity;
using Atrasti.Data.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Net;
using Atrasti.Middleware;
using Atrasti.Search;
using Atrasti.Services;
using Atrasti.Utils;
using Newtonsoft.Json;

namespace Atrasti.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConstructionRepository _constructionRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<AtrastiUser> _userManager;
        private readonly SearchService _searchService;

        public HomeController(ISubscriptionRepository subscriptionRepository,
            IProductRepository productRepository, IUserRepository userRepository, IEmailSender emailSender,
            UserManager<AtrastiUser> userManager, IConstructionRepository constructionRepository, SearchService searchService)
        {
            _subscriptionRepository = subscriptionRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _emailSender = emailSender;
            _userManager = userManager;
            _constructionRepository = constructionRepository;
            _searchService = searchService;
        }

        [HttpGet]
        public IActionResult Construction()
        {
            return View("Construction");
        }

        [HttpPost]
        public async Task<JsonResult> Construction(string emailAddress)
        {
            string remoteIpAddress = Request.Headers["X-Forwarded-For"];

            string requestUrl = $"http://ip-api.com/json/{remoteIpAddress}?fields=status,continent,country";
            string json = new WebClient().DownloadString(requestUrl);
            ContinentBlockMiddleWare.ContinentJsonResponse jsonResponse =
                JsonConvert.DeserializeObject<ContinentBlockMiddleWare.ContinentJsonResponse>(json);

            bool inserted =
                await _constructionRepository.SignUpEmailForNotificationAsync(emailAddress, jsonResponse.continent);
            
            return Json(new
            {
                result = inserted ? "OK" : "FAILED"
            });
        }

        public async Task<IActionResult> Index()
        {
            IDictionary<string, Subscription> subscriptions = await _subscriptionRepository.FetchSubscriptions();

            if (subscriptions.TryGetValue("search_hero", out Subscription subscription))
                ViewData["hero"] = subscription;

            if (subscriptions.TryGetValue("index_twenty", out Subscription productSubscription))
            {
                int[] ids = Array.ConvertAll(productSubscription.Data.Split(","), int.Parse);
                ICollection<AtrastiUser> users = await _userRepository.FindSetupUsersAsync(ids);
                foreach (AtrastiUser user in users)
                {
                    ICollection<Product> products = await _productRepository.FindProductsByCompanyIdAsync(user.Id);
                    ICollection<Product> singleProduct = new List<Product>();
                    if (products.Count > 0)
                        singleProduct.Add(products.Last());
                    user.CompanyModel.Products = singleProduct;
                }

                ViewData["index_twenty"] = users;
            }
            else
            {
                ViewData["index_twenty"] = new List<AtrastiUser>();
            }

            ViewData["user"] = await _userManager.GetUserAsync(User);
            return View();
        }

        [HttpGet]
        public IActionResult SearchResult()
        {
            ViewData["companies"] = new List<AtrastiUser>();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SearchResult(string searchString)
        {
            if (searchString == null)
            {
                ViewData["companies"] = new List<AtrastiUser>();
                return View();
            }

            ICollection<AtrastiUser> companies;
            IReadOnlyCollection<SearchDocument> searchDocuments = await _searchService.SearchProducts("products", searchString);
            if (searchDocuments.Count > 0)
            {
                IDictionary<int, SearchAlgorithm.SearchResult> searchResults =
                    new Dictionary<int, SearchAlgorithm.SearchResult>();
            
                foreach (SearchDocument product in searchDocuments)
                {
                    if (!searchResults.ContainsKey(product.CompanyId))
                    {
                        searchResults.Add(product.CompanyId, new SearchAlgorithm.SearchResult()
                        {
                            Title = product.Title
                        });
                    }
                }
                
                companies = await _userRepository.FindSetupUsersAsync(searchResults.Keys.ToArray());
            }
            else
            {
                companies = new List<AtrastiUser>();
            }
            
            ViewData["companies"] = companies;
            return View();
        }

        public async Task<IActionResult> Discover()
        {
            ICollection<AtrastiUser> companies = await _userRepository.FindTop10Users();
            ViewData["companies"] = companies;
            return View("Discover");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        [HttpGet]
        public IActionResult About()
        {
            return View("About");
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View("Contact");
        }


        [HttpPost]
        public async Task<IActionResult> Contact(string name, string email, string subject, string message)
        {
            await _emailSender.SendEmailAsync("atrasti@outlook.com", email + ": " + subject, name + ": " + message);
            return View("Contact");
        }

        [HttpGet]
        public async Task<IActionResult> Popular(string type)
        {
            if (type == null)
            {
                ViewData["companies"] = new List<AtrastiUser>();
                return View("SearchResult");
            }

            ICollection<AtrastiUser> companies;
            IReadOnlyCollection<SearchDocument> searchDocuments = await _searchService.SearchProducts("products", type);
            if (searchDocuments.Count > 0)
            {
                IDictionary<int, SearchAlgorithm.SearchResult> searchResults =
                    new Dictionary<int, SearchAlgorithm.SearchResult>();
            
                foreach (SearchDocument product in searchDocuments)
                {
                    if (!searchResults.ContainsKey(product.CompanyId))
                    {
                        searchResults.Add(product.CompanyId, new SearchAlgorithm.SearchResult()
                        {
                            Title = product.Title
                        });
                    }
                }
                
                companies = await _userRepository.FindSetupUsersAsync(searchResults.Keys.ToArray());
            }
            else
            {
                companies = new List<AtrastiUser>();
            }
            
            ViewData["companies"] = companies;
            return View("SearchResult");
        }
    }
}