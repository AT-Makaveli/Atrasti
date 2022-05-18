using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Atrasti.Controllers
{
    public class ProductController : Controller
    {
        [HttpPost]
        public async Task<JsonResult> AddProduct(string product)
        {
            if (!User.Identity.IsAuthenticated)
            {
                //return RedirectToAction("Index", "Home");
            }
            return Json(new { result = "success" });
        }
    }
}