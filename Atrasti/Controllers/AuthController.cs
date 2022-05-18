using System;
using System.Threading.Tasks;
using Atrasti.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Atrasti.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<AtrastiUser> _userManager;

        public AuthController(UserManager<AtrastiUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<JsonResult> ChangePassword(string currentPassword, string newPassword)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { errorCode = -1 });
            }
            
            AtrastiUser user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { errorCode = -1 });
            }

            IdentityResult result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

            if (!result.Succeeded)
            {
                return Json(new
                {
                    errorCode = -1,
                    errors = result.Errors
                });
            }

            return Json(new
            {
                errorCode = 0
            });
        }
    }
}