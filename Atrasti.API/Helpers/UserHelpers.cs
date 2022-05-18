using Atrasti.API.Models.User;
using Atrasti.Data.Models;

namespace Atrasti.API.Helpers
{
    public static class UserHelpers
    {
        public static User_Res MapUserModel(this AtrastiUser user)
        {
            return new User_Res(user.FirstName, user.LastName, user.Company, user.CompanyLogo);
        }
    }
}