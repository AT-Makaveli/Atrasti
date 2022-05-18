using System.ComponentModel.DataAnnotations;

namespace Atrasti.API.Models.User
{
    public class Login_Req
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}