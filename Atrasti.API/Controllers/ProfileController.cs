using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Atrasti.API.Helpers;
using Atrasti.API.Models.Company;
using Atrasti.API.Models.ProductModels;
using Atrasti.API.Models.Profile;
using Atrasti.Data.Core;
using Atrasti.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;

namespace Atrasti.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ProfileController : Controller
    {
        private readonly IAgentRepository _agentRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IProductRepository _productRepository;
        private readonly UserManager<AtrastiUser> _userManager;

        public ProfileController(IAgentRepository agentRepository, ICompanyRepository companyRepository,
            IProductRepository productRepository, UserManager<AtrastiUser> userManager)
        {
            _agentRepository = agentRepository;
            _companyRepository = companyRepository;
            _productRepository = productRepository;
            _userManager = userManager;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ProfilePage([FromBody] ProfilePage_Req profilePage)
        {
            AtrastiUser user;
            bool isProfileOwner = false;
            if (profilePage.UserId != null)
            {
                user = await _userManager.FindByIdAsync(profilePage.UserId);
            }
            else
            {
                user = await _userManager.GetUserAsync(User);
                isProfileOwner = true;
            }

            if (user == null)
                return BadRequest(new InvalidProfileModelError(InvalidProfileModelError.USER_NOT_SET, "User not set."));

            if (user.CompanyInfoSetup && user.CompanySetup)
            {
                ICollection<Product> products = await _productRepository.FindProductsByCompanyIdAsync(user.Id);
                ICollection<Product_Res> productResults = new List<Product_Res>();
                foreach (Product product in products)
                {
                    if (product.ProductLikes.Contains(user.Id)) product.IsHeartPressed = true;
                    productResults.Add(product.MapProductModel());
                }

                return Ok(new ProfilePage_Res()
                {
                    Setup = true,
                    IsProfileOwner = isProfileOwner,
                    CompanyPage = new CompanyPage_Res()
                    {
                        Products = productResults
                    },
                    User = user.MapUserModel(),
                });
            }

            Agent agent = await _agentRepository.FindAgentProfileByIdAsync(user.Id);
            if (agent != null)
            {
                return Ok(new ProfilePage_Res()
                {
                    Setup = true,
                    IsProfileOwner = isProfileOwner,
                    AgentPage = agent.MapAgent(user.CompanyLogo),
                    User = user.MapUserModel(),
                });
            }

            int userType = 0;
            if (user.Company != null)
            {
                userType = 1;
            }

            return Ok(new ProfilePage_Res()
            {
                Setup = false,
                UserType = userType
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> LikeInteract([FromBody] ProductLike_Req req)
        {
            AtrastiUser user = await _userManager.GetUserAsync(User);

            Product product = await _productRepository.FindProductByIdAsync(req.ProductId);
            if (product == null)
                return BadRequest(new InvalidProfileModelError(InvalidProfileModelError.PRODUCT_NOT_SET,
                    "Product does not exist!"));

            if (product.ProductLikes.Contains(user.Id))
            {
                product.ProductLikes.Remove(user.Id);
                await _productRepository.UnlikeProductAsync(new ProductLike()
                {
                    RefId = product.Id,
                    UserId = user.Id
                });
                product.IsHeartPressed = false;
            }
            else
            {
                product.ProductLikes.Add(user.Id);
                await _productRepository.LikeProductAsync(new ProductLike()
                {
                    RefId = product.Id,
                    UserId = user.Id
                });
                product.IsHeartPressed = true;
            }

            return Ok(product.MapProductModel());
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ManageProfilePage()
        {
            AtrastiUser user = await _userManager.GetUserAsync(User);
            Company_Res companyRes = null;
            CompanyInfo_Res companyInfoRes = null;
            if (user.CompanySetup && user.CompanyInfoSetup)
            {
                var company = await _companyRepository.FindCompanyByIdAsync(user.Id);
                companyRes = company.MapCompany();
                companyRes.PhoneNumber = user.PhoneNumber;
                companyInfoRes = company.CompanyInfo.MapCompanyInfo();
            }

            return Ok(new ManageProfile_Res()
            {
                Company = companyRes,
                CompanyInfo = companyInfoRes
            });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ManageAgentPage()
        {
            AtrastiUser user = await _userManager.GetUserAsync(User);
            Agent agent = await _agentRepository.FindAgentProfileByIdAsync(user.Id);

            if (agent != null) return Ok(agent.MapAgent(user.CompanyLogo));

            return Ok(null);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ManageAgentPage([FromBody] ManageAgent_Req req)
        {
            AtrastiUser user = await _userManager.GetUserAsync(User);

            if (!string.IsNullOrEmpty(req.Logo))
            {
                byte[] b64Data = Convert.FromBase64String(req.Logo);
                if (ProductController.CheckIfImageFile(b64Data))
                {
                    string fileName = user.Id + ".png";
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/logos", fileName);
                    await System.IO.File.WriteAllBytesAsync(path, b64Data);
                    user.CompanyLogo = fileName;

                    await _userManager.UpdateAsync(user);
                }
            }

            Agent agent = await _agentRepository.FindAgentProfileByIdAsync(user.Id);
            if (agent == null)
            {
                agent = req.MapCreateAgent(user.Id);
                await _agentRepository.CreateAgentAsync(agent);
            }
            else
            {
                agent = req.MapUpdateAgent(agent);
                await _agentRepository.UpdateAgentAsync(agent);
            }

            return Ok(agent.MapAgent(user.CompanyLogo));
        }
    }
}