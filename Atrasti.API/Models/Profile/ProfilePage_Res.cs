using Atrasti.API.Models.User;

namespace Atrasti.API.Models.Profile
{
    public class ProfilePage_Res
    {
        public bool Setup { get; set; }
        public bool IsProfileOwner { get; set; }
        public User_Res User { get; set; }
        public CompanyPage_Res CompanyPage { get; set; }
        public AgentPage_Res AgentPage { get; set; }
        public int UserType { get; set; }
    }
}