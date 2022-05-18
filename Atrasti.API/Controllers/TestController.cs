using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Atrasti.API.Controllers
{
    public class TestController : Controller
    {
        [Authorize]
        [HttpPost("/test/signout")]
        public async Task<IActionResult> SignOut()
        {
            Console.WriteLine(User);
            return Ok(new
            {
                result = "Succeeded"
            });
        }
    }
}