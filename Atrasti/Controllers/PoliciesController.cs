using Microsoft.AspNetCore.Mvc;

namespace Atrasti.Controllers
{
    public class PoliciesController : Controller
    {
        public IActionResult PrivacyPolicy()
        {
            return View();
        }

        public IActionResult TermsOfUse()
        {
            return View();
        }

        public IActionResult Cookies()
        {
            return View();
        }
    }
}