using System;

namespace Atrasti.API.Models.Token
{
    public class UserToken
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public TimeSpan ValidTo { get; set; }
    }
}