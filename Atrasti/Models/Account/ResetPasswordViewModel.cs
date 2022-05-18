using System.ComponentModel.DataAnnotations;

namespace Atrasti.Models.Account
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string Email { get; set; }

        public string Token { get; set; }

        public string Id { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
