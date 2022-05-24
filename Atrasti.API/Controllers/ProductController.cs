using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Atrasti.API.Helpers;
using Atrasti.API.Models.ProductModels;
using Atrasti.Data.Core;
using Atrasti.Data.Models;
using Atrasti.Data.Models.Users;
using Atrasti.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Atrasti.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ProductController : Controller
    {
        private readonly IBaseCategoriesRepository _baseCategoriesRepository;
        private readonly IProductRepository _productRepository;
        private readonly SearchService _searchService;
        private readonly UserManager<AtrastiUser> _userManager;

        public ProductController(IProductRepository productRepository, UserManager<AtrastiUser> userManager,
            SearchService searchService, IBaseCategoriesRepository baseCategoriesRepository)
        {
            _productRepository = productRepository;
            _userManager = userManager;
            _searchService = searchService;
            _baseCategoriesRepository = baseCategoriesRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UserCategories()
        {
            AtrastiUser user = await _userManager.GetUserAsync(User);
            if (user.UserType == UserType.Company)
            {
                IList<BaseCategory> categories = await _baseCategoriesRepository.FindUserCategories(user.Id);

                return Ok(new ProductCategories_Res()
                {
                    Categories = categories.MapCategoriesRes()
                });
            }

            return BadRequest();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UploadProduct([FromBody] UploadProduct_Req req)
        {
            if (string.IsNullOrEmpty(req.Image))
                return BadRequest(new InvalidProductModelError(InvalidProductModelError.PRODUCT_IMAGE_NOT_SET,
                    "Must contain image."));

            if (string.IsNullOrEmpty(req.Title))
                return BadRequest(new InvalidProductModelError(InvalidProductModelError.PRODUCT_TITLE_NOT_SET,
                    "Must contain title."));

            if (string.IsNullOrEmpty(req.Description))
                return BadRequest(new InvalidProductModelError(InvalidProductModelError.PRODUCT_DESC_NOT_SET,
                    "Must contain description."));

            if (req.Tags.Length == 0)
                return BadRequest(new InvalidProductModelError(InvalidProductModelError.PRODUCT_TAGS_NOT_SET,
                    "Must contain tags."));

            AtrastiUser user = await _userManager.GetUserAsync(User);
            Product product = new Product
            {
                CompanyId = user.Id,
                Title = req.Title,
                Description = req.Description,
                Tags = req.Tags.ToList(),
                PhoneticTags = req.Tags.ToList(),
                ProductCategory = req.Category
            };

            await _productRepository.CreateProductAsync(product);

            byte[] b64Data = Convert.FromBase64String(req.Image);
            if (CheckIfImageFile(b64Data))
            {
                string fileName = product.Id + ".png";
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/products", fileName);
                await System.IO.File.WriteAllBytesAsync(path, b64Data);
            }

            await _searchService.IndexDocument("products", new SearchDocument()
            {
                DocumentId = product.Id.ToString(),
                CompanyId = user.Id,
                Title = req.Title,
                Tags = req.Tags,
                Description = req.Description
            });

            return Ok();
        }

        internal static bool CheckIfImageFile(byte[] fileBytes)
        {
            return WriterHelper.GetImageFormat(fileBytes) != WriterHelper.ImageFormat.Unknown;
        }
    }
}