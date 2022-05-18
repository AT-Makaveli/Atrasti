using Microsoft.AspNetCore.Mvc;

namespace Atrasti.Controllers
{
    public class InviteController : Controller
    {
        // GET
        public IActionResult Index(int id)
        {
            TempData["referrer"] = id;
            return RedirectToAction("Index", "Register");
        }
    }
}