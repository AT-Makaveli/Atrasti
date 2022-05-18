using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using Atrasti.API.Helpers;
using Atrasti.API.Models.Error;
using Atrasti.API.Models.Token;
using Atrasti.API.Models.User;
using Atrasti.API.Services;
using Atrasti.API.Services.Jwt;
using Atrasti.API.Services.Refresh;
using Atrasti.Data.Core;
using Atrasti.Data.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Atrasti.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly IJwtService _jwtService;
        private readonly IRefreshService _refreshService;
        private readonly IUserRepository _userRepository;
        private readonly IUserDataRepository _userDataRepository;
        private readonly UserManager<AtrastiUser> _userManager;
        private readonly SignInManager<AtrastiUser> _signInManager;

        public AuthController(IEmailSender emailSender, IJwtService jwtService, IRefreshService refreshService,
            IUserRepository userRepository, IUserDataRepository userDataRepository,
            UserManager<AtrastiUser> userManager, SignInManager<AtrastiUser> signInManager)
        {
            _emailSender = emailSender;
            _jwtService = jwtService;
            _refreshService = refreshService;
            _userRepository = userRepository;
            _userDataRepository = userDataRepository;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login_Req loginReq)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
                await LogOut();

            if (!ModelState.IsValid)
                return BadRequest(new InvalidLoginModelError(InvalidLoginModelError.AUTH_ALL_FIELDS,
                    "Please fill in all fields."));

            var result =
                await _signInManager.PasswordSignInAsync(loginReq.Email, loginReq.Password, true,
                    lockoutOnFailure: false);
            if (!result.Succeeded)
                return BadRequest(new InvalidLoginModelError(InvalidLoginModelError.INVALID_EMAIL_OR_PASSWORD,
                    "Invalid email or password"));

            AtrastiUser user = await _userManager.FindByEmailAsync(loginReq.Email);
            string refreshToken = _refreshService.GenerateRefreshToken();
            await _userDataRepository.UpdateRefreshToken(new UserData()
            {
                UserId = user.Id,
                RefreshToken = refreshToken
            });
            UserToken userToken = _jwtService.GenerateUserToken(user);
            userToken.RefreshToken = refreshToken;

            return Ok(userToken);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] Register_Req req)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(req.Company) && await _userRepository.FindByCompany(req.Company))
            {
                errors.Add(InvalidRegisterModelError.COMPANY_IN_USE, "Company is in use.");
            }

            if (string.IsNullOrEmpty(req.Email))
                errors.Add(InvalidRegisterModelError.EMAIL_EMPTY, "Email must be set.");

            if (!IsValidEmail(req.Email))
                errors.Add(InvalidRegisterModelError.EMAIL_INVALID, "Email must be valid.");

            if (await _userManager.FindByEmailAsync(req.Email) != null)
                errors.Add(InvalidRegisterModelError.EMAIL_IN_USE, "Email is in use.");

            if (string.IsNullOrEmpty(req.FirstName))
                errors.Add(InvalidRegisterModelError.FIRST_NAME_EMPTY, "The first name field is required.");

            if (string.IsNullOrEmpty(req.LastName))
                errors.Add(InvalidRegisterModelError.LAST_NAME_EMPTY, "The last name field is required.");

            if (errors.Count > 0)
            {
                BaseError baseError = new BaseError();
                foreach (KeyValuePair<string, string> error in errors)
                {
                    baseError.Errors.Add(new ErrorEntry(error.Key, error.Value));
                }

                return BadRequest(baseError);
            }

            AtrastiUser user = new AtrastiUser
            {
                Company = req.Company,
                Email = req.Email,
                UserName = req.Email,
                FirstName = req.FirstName[0].ToString().ToUpper() + req.FirstName.Substring(1),
                LastName = req.LastName[0].ToString().ToUpper() + req.LastName.Substring(1),
            };

            bool validPassword =
                (await _userManager.PasswordValidators[0].ValidateAsync(_userManager, user, req.Password)).Succeeded;
            if (!validPassword)
                errors.Add(InvalidRegisterModelError.PASSWORD_INVALID,
                    "Password between 8 and 20 characters; must contain at least one lowercase letter, one uppercase letter, one numeric digit, and one special character.");

            if (errors.Count > 0)
            {
                BaseError baseError = new BaseError();
                foreach (KeyValuePair<string, string> error in errors)
                {
                    baseError.Errors.Add(new ErrorEntry(error.Key, error.Value));
                }

                return BadRequest(baseError);
            }

            IdentityResult result = await _userManager.CreateAsync(user, req.Password);

            if (result.Succeeded) return Ok();

            return BadRequest(new InvalidLoginModelError(InvalidRegisterModelError.UNKNOWN_ERROR,
                "Unknown error, please contact us."));
        }

        [HttpPost]
        public async Task<IActionResult> ValidCompany([FromBody] ValidCompany_Req req)
        {
            if (string.IsNullOrEmpty(req.Company))
            {
                return BadRequest(new InvalidRegisterModelError(InvalidRegisterModelError.COMPANY_EMPTY,
                    "Company must be set."));
            }

            if (await _userRepository.FindByCompany(req.Company))
            {
                return BadRequest(new InvalidRegisterModelError(InvalidRegisterModelError.COMPANY_IN_USE,
                    "Company is in use."));
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> ValidEmail([FromBody] ValidEmail_Req req)
        {
            if (string.IsNullOrEmpty(req.Email))
            {
                return BadRequest(new InvalidRegisterModelError(InvalidRegisterModelError.EMAIL_EMPTY,
                    "Email must be set."));
            }

            if (!IsValidEmail(req.Email))
            {
                return BadRequest(new InvalidRegisterModelError(InvalidRegisterModelError.EMAIL_INVALID,
                    "Email must be valid."));
            }

            if (await _userManager.FindByEmailAsync(req.Email) != null)
            {
                return BadRequest(new InvalidRegisterModelError(InvalidRegisterModelError.EMAIL_IN_USE,
                    "Email is in use."));
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPassword_Req req)
        {
            if (string.IsNullOrEmpty(req.Email))
            {
                return BadRequest(new InvalidForgotPasswordModelError(InvalidForgotPasswordModelError.EMAIL_EMPTY,
                    "Email must be set."));
            }

            if (!IsValidEmail(req.Email))
            {
                return BadRequest(new InvalidForgotPasswordModelError(InvalidForgotPasswordModelError.EMAIL_INVALID,
                    "Email must be valid."));
            }

            bool sent = false;
            AtrastiUser user = await _userManager.FindByEmailAsync(req.Email);
            if (user != null && user.EmailConfirmed)
            {
                string token = await _userManager.GeneratePasswordResetTokenAsync(user);
                sent = await _emailSender.SendEmailAsync(req.Email,
                    "Reset your password",
                    ("http://test.atrasti.com?user=" + user.Id + "&token=" + token).ResetPasswordTemplate(), true);
            }

            return Ok(new ForgotPassword_Res()
            {
                Status = sent
            });
        }

        private static bool IsValidEmail(string email)
        {
            if (MailAddress.TryCreate(email, out _))
            {
                return true;
            }

            return false;
        }
    }
}