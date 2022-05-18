using System.Threading.Tasks;
using Atrasti.API.Models.User;
using Atrasti.Data.Core;
using Atrasti.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Atrasti.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserDataController : Controller
    {
        private readonly UserManager<AtrastiUser> _userManager;
        private readonly IUserDataRepository _userDataRepository;

        public UserDataController(UserManager<AtrastiUser> userManager, IUserDataRepository userDataRepository)
        {
            _userManager = userManager;
            _userDataRepository = userDataRepository;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SetFcmToken([FromBody] SetFcmToken_Req req)
        {
            AtrastiUser user = await _userManager.GetUserAsync(User);

            if (string.IsNullOrEmpty(req.FcmToken))
            {
                return BadRequest(new InvalidUserModelError(InvalidUserModelError.FCM_TOKEN_NOT_SET,
                    "Fcm token can't be empty."));
            }

            var userData = new UserData
            {
                UserId = user.Id,
                FcmToken = req.FcmToken
            };

            await _userDataRepository.UpdateUserData(userData);

            return Ok();
        }
    }
}