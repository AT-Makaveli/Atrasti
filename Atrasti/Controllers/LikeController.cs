using Atrasti.Data.Core;
using Atrasti.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Atrasti.Controllers
{
    public class LikeController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly UserManager<AtrastiUser> _userManager;

        public LikeController(IProductRepository productRepository, UserManager<AtrastiUser> userManager)
        {
            _productRepository = productRepository;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<JsonResult> LikePost(int itemId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { result = "failed" });
            }

            AtrastiUser user = await _userManager.GetUserAsync(User);
            Product product = await _productRepository.FindProductByIdAsync((uint)itemId);
            if (product.ProductLikes.Contains(user.Id))
            {
                return Json(new { result = "failed" });
            }
            await _productRepository.LikeProductAsync(new ProductLike()
            {
                RefId = (uint)itemId,
                UserId = user.Id
            });

            return Json(new { result = "success" });
        }

        [HttpPost]
        public async Task<JsonResult> UnlikePost(int itemId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { result = "failed" });
            }

            AtrastiUser user = await _userManager.GetUserAsync(User);
            Product product = await _productRepository.FindProductByIdAsync((uint)itemId);
            if (!product.ProductLikes.Contains(user.Id))
            {
                return Json(new { result = "failed" });
            }

            await _productRepository.UnlikeProductAsync(new ProductLike()
            {
                RefId = (uint)itemId,
                UserId = user.Id
            });

            return Json(new { result = "success" });
        }
    }
}