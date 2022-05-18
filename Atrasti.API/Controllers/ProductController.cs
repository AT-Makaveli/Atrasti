using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Atrasti.API.Helpers;
using Atrasti.API.Models.ProductModels;
using Atrasti.Data.Core;
using Atrasti.Data.Models;
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
        private readonly IProductRepository _productRepository;
        private readonly SearchService _searchService;
        private readonly UserManager<AtrastiUser> _userManager;

        public ProductController(IProductRepository productRepository, UserManager<AtrastiUser> userManager,
            SearchService searchService)
        {
            _productRepository = productRepository;
            _userManager = userManager;
            _searchService = searchService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UploadProduct([FromBody] UploadProduct_Req req)
        {
            if (req.Image.Length == 0)
                return BadRequest(new InvalidProductModelError(InvalidProductModelError.PRODUCT_IMAGE_NOT_SET,
                    "Must contain image."));

            if (req.Title.Length == 0)
                return BadRequest(new InvalidProductModelError(InvalidProductModelError.PRODUCT_TITLE_NOT_SET,
                    "Must contain title."));

            if (req.Description.Length == 0)
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
                PhoneticTags = req.Tags.ToList()
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