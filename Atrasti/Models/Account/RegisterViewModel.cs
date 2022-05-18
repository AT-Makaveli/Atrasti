using System.ComponentModel.DataAnnotations;

namespace Atrasti.Models.Account
{
    public class RegisterViewModel
    {
        public string Company { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}