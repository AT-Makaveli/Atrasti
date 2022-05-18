using Atrasti.Data.Core;
using Atrasti.Data.Models;
using Atrasti.Models.Profile;
using Atrasti.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Atrasti.Search;
using Phonix;

namespace Atrasti.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IChatRepository _chatRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly UserManager<AtrastiUser> _userManager;
        private readonly SearchService _searchService;

        public ProfileController(ICompanyRepository companyRepository, IUserRepository userRepository,
            IProductRepository productRepository, UserManager<AtrastiUser> userManager, IChatRepository chatRepository, SearchService searchService)
        {
            _companyRepository = companyRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
            _userManager = userManager;
            _chatRepository = chatRepository;
            _searchService = searchService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Login",
                    new {returnUrl = Url.RouteUrl( "Profile", new {id})});

            AtrastiUser user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            bool isProfileOwner = User.Identity.IsAuthenticated && User.Identity.Name == user.Email;
            if (user.CompanyInfoSetup && user.CompanySetup)
            {
                user = await _userRepository.FindSetupUserById(id);
                ViewData["user"] = user;
                ViewData["profileOwner"] = isProfileOwner;
                ICollection<Product> products = await _productRepository.FindProductsByCompanyIdAsync(user.Id);
                ViewData["products"] = products;
                ViewData["referral_link"] = Url.RouteUrl("Invite", new {user.Id}, protocol: Request.Scheme);

                
                return View("Index");
            }

            if (isProfileOwner)
            {
                ViewData["user"] = user;
                ViewData["profileOwner"] = true;
                return View("GetStarted");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Management()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            AtrastiUser user = await _userManager.GetUserAsync(User);
            if (user.CompanyInfoSetup && user.CompanySetup)
            {
                user = await _userRepository.FindSetupUserById(user.Id);
            }

            //IList<ChatFriend> friends = await _chatRepository.FetchFriendsAsync(user.Id);
            ViewData["user"] = user;
            //ViewData["friends"] = friends;
            return View("Management");
        }

        [HttpPost]
        public async Task<IActionResult> ProductPost(ProductModel productModel)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            AtrastiUser user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            Product product = new Product();

            if (productModel.Tags != null)
            {
                product.Tags = JsonConvert.DeserializeObject<List<string>>(productModel.Tags);

                IList<string> phoneticTags = new List<string>();
                DoubleMetaphone doubleMetaphone = new DoubleMetaphone(6);
                foreach (string tag in productModel.Tags.Split(','))
                {
                    phoneticTags.Add(doubleMetaphone.BuildKey(tag));
                }

                product.PhoneticTags = phoneticTags;
            }

            if (productModel.Title != null)
            {
                product.Title = productModel.Title;
            }

            if (productModel.Description != null)
            {
                product.Description = productModel.Description;
            }

            product.CompanyId = user.Id;
            await _productRepository.CreateProductAsync(product);
            bool result = await _searchService.IndexDocument("products", new SearchDocument()
            {
                CompanyId = user.Id,
                Title = user.Company,
                ProductTitle = product.Title,
                Tags = product.Tags.ToArray()
            });
            
            if (productModel.Image != null)
            {
                var base64Data = Regex.Match(productModel.Image, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"]
                    .Value;
                byte[] b64Data = Convert.FromBase64String(base64Data);
                if (CheckIfImageFile(b64Data))
                {
                    string fileName = product.Id + ".png";
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/products", fileName);
                    await System.IO.File.WriteAllBytesAsync(path, b64Data);
                }
            }

            return Redirect(Url.RouteUrl("Profile", new {id = user.Id}));
        }

        [HttpPost]
        public async Task<IActionResult> AccountSettings(AccountSettingsModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            AtrastiUser user = await _userManager.GetUserAsync(User);

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;

            if (model.CompanyImage != null)
            {
                var base64Data = Regex.Match(model.CompanyImage, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"]
                    .Value;
                byte[] b64Data = Convert.FromBase64String(base64Data);
                if (CheckIfImageFile(b64Data))
                {
                    if (user.CompanyLogo != null)
                    {
                        var pathToFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/logos",
                            user.CompanyLogo + ".png");
                        if (System.IO.File.Exists(pathToFile))
                            System.IO.File.Delete(pathToFile);
                    }
                    
                    string fileName = Guid.NewGuid() + ".png";
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/logos", fileName);
                    await System.IO.File.WriteAllBytesAsync(path, b64Data);
                    user.CompanyLogo = fileName;
                }
            }

            await _userManager.UpdateAsync(user);
            IActionResult result = await Management();
            return result;
        }

        [HttpPost]
        public async Task<IActionResult> ProfileSettings(AccountSettingsModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            AtrastiUser user = await _userManager.GetUserAsync(User);
            Company company = await _companyRepository.FindCompanyByIdAsync(user.Id);
            if (!string.IsNullOrEmpty(model.CompanyDesc) && model.CompanyDesc.Length > 167)
            {
                model.CompanyDesc = model.CompanyDesc.Substring(0, 167);
            }

            if (string.IsNullOrEmpty(model.Certificates))
            {
                model.Certificates = "";
            }
            
            if (company == null)
            {
                company = new Company()
                {
                    RefId = user.Id,
                    Address = model.Address,
                    City = model.City,
                    Country = model.Country,
                    Website = model.Website,
                    CompanyDesc = model.CompanyDesc
                };

                user.PhoneNumber = model.PhoneNumber;

                company.CompanyInfo = new CompanyInfo()
                {
                    RefId = user.Id,
                    BusinessType = model.BusinessType,
                    MainMarkets = model.MainMarkets,
                    MainProducts = model.MainProducts,
                    YearEstablished = model.YearEstablished,
                    Certificates = model.Certificates,
                    Capacity = model.Capacity
                };

                user.CompanyInfoSetup = true;
                user.CompanySetup = true;

                await _companyRepository.CreateCompanyAsync(company);
                await _userManager.UpdateAsync(user);
            }
            else
            {
                company.Address = model.Address;
                company.City = model.City;
                user.PhoneNumber = model.PhoneNumber;
                company.Country = model.Country;
                company.Website = model.Website;
                company.CompanyDesc = model.CompanyDesc;

                company.CompanyInfo.BusinessType = model.BusinessType;
                company.CompanyInfo.MainProducts = model.MainProducts;
                company.CompanyInfo.MainMarkets = model.MainMarkets;
                company.CompanyInfo.YearEstablished = model.YearEstablished;
                company.CompanyInfo.Certificates = model.Certificates;
                company.CompanyInfo.Capacity = model.Capacity;

                user.CompanyInfoSetup = true;
                user.CompanySetup = true;

                await _userManager.UpdateAsync(user);
                await _companyRepository.UpdateCompanyAsync(company);
            }

            IActionResult result = await Management();
            return result;
        }

        internal static bool CheckIfImageFile(byte[] fileBytes)
        {
            return WriterHelper.GetImageFormat(fileBytes) != WriterHelper.ImageFormat.unknown;
        }

        [NonAction]
        public async Task<IActionResult> Chat(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            AtrastiUser user = await _userManager.GetUserAsync(User);
            if (user.CompanyInfoSetup && user.CompanySetup)
            {
                user = await _userRepository.FindSetupUserById(user.Id);
            }

            int chatId = user.Id.CombineIntToHash(id);

            AtrastiUser friend = await _userRepository.FindUserById(id);
            IList<ChatFriend> friends = await _chatRepository.FetchFriendsAsync(user.Id);
            IList<ChatMessage> messages = await _chatRepository.FetchMessagesAsync(chatId);
            ViewData["user"] = user;
            ViewData["friends"] = friends;
            ViewData["friend"] = friend;
            ViewData["messages"] = messages;
            return View("Chat");
        }
        
        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            AtrastiUser user = await _userManager.GetUserAsync(User);
            if (user.CompanyInfoSetup && user.CompanySetup)
            {
                user = await _userRepository.FindSetupUserById(user.Id);
            }

            //IList<ChatFriend> friends = await _chatRepository.FetchFriendsAsync(user.Id);
            ViewData["user"] = user;
            //ViewData["friends"] = friends;
            return View("ChangePassword");
        }

        [HttpPost]
        public async Task<JsonResult> DeleteProduct(string itemId)
        {
            if (User.Identity.IsAuthenticated && uint.TryParse(itemId, out uint productId))
            {
                AtrastiUser user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                     Product product = await _productRepository.FindProductByIdAsync(productId);
                     if (product != null && product.CompanyId == user.Id)
                     {
                         await _productRepository.RemoveProduct(productId);
                         return Json(new
                         {
                             status = "success"
                         });
                     }
                }
            } 
            
            return Json(new
            {
                status = "error"
            });
        }
    }
}