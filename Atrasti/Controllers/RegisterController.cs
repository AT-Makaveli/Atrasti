using Atrasti.Data;
using Atrasti.Data.Models;
using Atrasti.Models;
using Atrasti.Models.Account;
using Atrasti.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Atrasti.Data.Core;
using Atrasti.Utils;

namespace Atrasti.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<AtrastiUser> _userManager;
        private readonly SignInManager<AtrastiUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IShorteningRepository _shorteningRepository;
        private readonly IUserDataRepository _userDataRepository;

        public RegisterController(
            UserManager<AtrastiUser> userManager,
            SignInManager<AtrastiUser> signInManager,
            IEmailSender emailSender,
            IUserDataRepository userDataRepository, IShorteningRepository shorteningRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _userDataRepository = userDataRepository;
            _shorteningRepository = shorteningRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewData["registerSuccess"] = false;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(RegisterViewModel rvm)
        {
            ViewData["registerSuccess"] = false;
            Dictionary<string, string> errors = new Dictionary<string, string>();
            ViewData.Add("old-Email", rvm.Email);
            ViewData.Add("old-Company", rvm.Company);
            ViewData.Add("old-FirstName", rvm.FirstName);
            ViewData.Add("old-LastName", rvm.LastName);

            if (string.IsNullOrEmpty(rvm.Company))
                errors.Add("invalid-Company", "The company field is required.");

            if (string.IsNullOrEmpty(rvm.Email) || IsValidEmail(rvm.Email))
                errors.Add("invalid-Email", "Please enter a valid email address.");

            if (string.IsNullOrEmpty(rvm.FirstName))
                errors.Add("invalid-FirstName", "The first name field is required.");

            if (string.IsNullOrEmpty(rvm.LastName))
                errors.Add("invalid-LastName", "The last name field is required.");

            if (rvm.Password != rvm.ConfirmPassword)
                errors.Add("invalid-ConfirmPassword", "Invalid password combination.");

            if (errors.Count > 0)
            {
                foreach (KeyValuePair<string, string> error in errors)
                {
                    ViewData.Add(error.Key, error.Value);
                }

                return View();
            }

            if (await _userManager.FindByEmailAsync(rvm.Email) != null)
            {
                errors.Add("invalid-Email", "Email is already in use.");
            }

            AtrastiUser user = new AtrastiUser
            {
                Company = rvm.Company,
                Email = rvm.Email,
                UserName = rvm.Email,
                FirstName = rvm.FirstName.First().ToString().ToUpper() + rvm.FirstName.Substring(1),
                LastName = rvm.LastName.First().ToString().ToUpper() + rvm.LastName.Substring(1),
                Referrer = TempData.Peek("referrer") as int?
            };
            
            bool validPassword = (await _userManager.PasswordValidators[0].ValidateAsync(_userManager, user, rvm.Password)).Succeeded;
            if (!validPassword)
                errors.Add("invalid-ConfirmPassword", "Password between 8 and 20 characters; must contain at least one lowercase letter, one uppercase letter, one numeric digit, and one special character");

            if (errors.Count > 0)
            {
                foreach (KeyValuePair<string, string> error in errors)
                {
                    ViewData.Add(error.Key, error.Value);
                }

                return View();
            }

            IdentityResult result = await _userManager.CreateAsync(user, rvm.Password);
            if (!result.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                foreach (IdentityError error in result.Errors)
                {
                    ViewData.Add(error.Code, error.Description);
                }
                
                return View();
            }

            if (result.Succeeded)
            {
                string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                string shortened = await _shorteningRepository.GenerateShortening(code);
                string callbackUrl = Url.Action(
                    action: nameof(ConfirmEmail),
                    controller: "Register",
                    values: new {user.Id, code = shortened},
                    protocol: Request.Scheme);
                
                bool succeed = await _emailSender.SendEmailAsync(rvm.Email,
                    "Atrasti - Confirm Account",
                    callbackUrl.ConfirmAccountTemplate(), true);
                if (succeed)
                {
                    ViewData["registerSuccess"] = true;
                }
                else
                {
                    await _userDataRepository.DeleteUserData(user.Id);
                }
            }

            return View();
        }

        public async Task<IActionResult> ConfirmEmail(string Id, string code)
        {
            if (Id == null || code == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{Id}'.");
            }

            Shortening shortening = await _shorteningRepository.GetOriginal(code);
            if (shortening == null)
            {
                throw new ApplicationException($"Unable to find shortening for '{code}'.");
            }
            
            var result = await _userManager.ConfirmEmailAsync(user, shortening.Original);
            if (result.Succeeded)
            {
                if (user.Referrer != null)
                {
                    await _userDataRepository.IncrementReferrals(user.Referrer.Value);
                }
                
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Home");
            }

            return View("Index");
        }

        private bool IsValidEmail(string email)
        {
            string pattern = @"^(?!\.)(""([^""\r\]|\[""\r\])*""|"
              + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
              + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(email);
        }

        private bool IsValidPassword(string password)
        {
            string pattern = "^.*(?=.{8,})(?=.*[\\d])(?=.*[\\W]).*$";
            return Regex.IsMatch(password, pattern);
        }
    }
}