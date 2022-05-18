namespace Atrasti.API.Models.User
{
    public class Register_Req
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string Password { get; set; }
    }
}