using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Atrasti.Spa.Controllers.Services;
using Atrasti.Spa.Models.View;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Atrasti.Data.Core;
using Atrasti.Data.Models;
using Atrasti.Spa.Models;
using Microsoft.AspNetCore.Identity;

namespace Atrasti.Spa.Controllers
{
    [Route("auth/[action]")]
    public class AuthController : Controller
    {
        private readonly AuthService _authService;
        private readonly UserManager<AtrastiUser> _userManager;
        private readonly SignInManager<AtrastiUser> _signInManager;
        private readonly IUserRepository _userRepository;

        public AuthController(AuthService authService, UserManager<AtrastiUser> userManager,
            SignInManager<AtrastiUser> signInManager, IUserRepository userRepository)
        {
            _authService = authService;
            _userManager = userManager;
            _signInManager = signInManager;
            _userRepository = userRepository;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginViewModel login)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            var result =
                await _signInManager.PasswordSignInAsync(login.Email, login.Password, true, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return Ok(); //Without redirect.
            }

            AtrastiUser user = await _userManager.FindByEmailAsync(login.Email);
            if (user != null && !user.EmailConfirmed)
            {
                ModelState.AddModelError(string.Empty, "Please verify your email");
                return Ok(new
                {
                    user.EmailConfirmed
                });
            }

            return Ok(new
            {
                invalid = "true"
            });
        }

        public async Task<ActionResult<bool>> Register([FromBody]RegisterViewModel rvm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            IDictionary<string, string> errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(rvm.Company))
                errors.Add("invalid-Company", "The company field is required.");

            if (string.IsNullOrEmpty(rvm.Email) || !IsValidEmail(rvm.Email))
                errors.Add("invalid-Email", "Please enter a valid email address.");

            if (string.IsNullOrEmpty(rvm.FirstName))
                errors.Add("invalid-FirstName", "The first name field is required.");

            if (string.IsNullOrEmpty(rvm.LastName))
                errors.Add("invalid-LastName", "The last name field is required.");
            
            if (errors.Count > 0)
            {
                foreach (KeyValuePair<string, string> error in errors)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }

                return BadRequest(ModelState);
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
                    ModelState.AddModelError(error.Key, error.Value);
                }

                return BadRequest(ModelState);
            }
            
            IdentityResult result = await _userManager.CreateAsync(user, rvm.Password);
            
            return result.Succeeded;
        }

        public async Task<JsonResult> ValidCompany([FromBody] RegisterViewModel rvm)
        {
            if (string.IsNullOrEmpty(rvm.Company))
            {
                return Json(new ValidateEmailData()
                {
                    Result = "empty"
                });
            }
            
            if (await _userRepository.FindByCompany(rvm.Company))
            {
                return Json(new ValidateEmailData()
                {
                    Result = "in-use"
                });
            }
            
            return Json(new ValidateEmailData()
            {
                Result = "success"
            });
        }
        
        public async Task<JsonResult> ValidEmail([FromBody] RegisterViewModel rvm)
        {
            if (string.IsNullOrEmpty(rvm.Email))
            {
                return Json(new ValidateEmailData()
                {
                    Result = "empty"
                });
            }
            
            if (!IsValidEmail(rvm.Email))
            {
                return Json(new ValidateEmailData()
                {
                    Result = "invalid"
                });
            }
            
            if (await _userManager.FindByEmailAsync(rvm.Email) != null)
            {
                return Json(new ValidateEmailData()
                {
                    Result = "in-use"
                });
            }
            
            return Json(new ValidateEmailData()
            {
                Result = "success"
            });
        }
        
        public static bool IsValidEmail(string email)
        {
            if (MailAddress.TryCreate(email, out MailAddress address))
            {
                return true;
            }

            return false;
        }
    }
}