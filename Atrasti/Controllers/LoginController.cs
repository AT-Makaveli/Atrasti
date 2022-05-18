using Atrasti.Data.Core;
using Atrasti.Data.Models;
using Atrasti.Models.Account;
using Atrasti.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Atrasti.Utils;

namespace Atrasti.Controllers
{
    public class LoginController : Controller
    {
        private readonly SignInManager<AtrastiUser> _signInManager;
        private readonly UserManager<AtrastiUser> _userManager;
        private readonly IEmailSender _emailSender;

        public LoginController(
            SignInManager<AtrastiUser> signInManager,
            UserManager<AtrastiUser> userManager,
            IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginViewModel loginViewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                ViewData.Add("old-email", loginViewModel.Email);
                
                var result = await _signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, loginViewModel.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }

                var user = await _userManager.FindByEmailAsync(loginViewModel.Email);
                if (user != null && !user.EmailConfirmed)
                {
                    ViewData.Add("invalid-login", "Please verify your email.");
                    ModelState.AddModelError(string.Empty, "Please verify your email");
                }
                else
                {
                    ViewData.Add("invalid-login", "Incorrect username/password");
                    ModelState.AddModelError(string.Empty, "Incorrect username/password");
                }

                return View(loginViewModel);
            }

            return View();
        }

        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View("ResetPassword");
        }

        [HttpPost]
        public async Task<IActionResult> ResetPasswordPost(ResetPasswordViewModel model)
        {
            AtrastiUser user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && user.EmailConfirmed)
            {
                string token = await _userManager.GeneratePasswordResetTokenAsync(user);
                string resetUrl = Url.Action("PasswordReset", "Login", new { userId = user.Id, code = token }, Request.Scheme);
                bool sent = await _emailSender.SendEmailAsync(model.Email,
                    "Reset your password",
                    resetUrl.ResetPasswordTemplate(), true);
            }

            ViewData["sent"] = true;
            return ResetPassword();
        }

        public async Task<IActionResult> PasswordReset(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }

            return View(new ResetPasswordViewModel()
            {
                Id = userId,
                Token = code
            });
        }

        [HttpPost]
        public async Task<IActionResult> PasswordReset(ResetPasswordViewModel model)
        {
            if (model.Password == model.ConfirmPassword)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user == null)
                {
                    throw new ApplicationException($"Unable to load user with ID '{model.Id}'.");
                }

                IdentityResult result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                if (!result.Succeeded)
                {
                    ViewData["password-error"] = result.Errors.ToArray()[0].Description;
                }
                else
                {
                    ViewData["password-succeed"] = true;
                }
            }
            else
            {
                ViewData["password-error"] = "The password combination failed!";
            }

            return View(model);
        }
    }
}