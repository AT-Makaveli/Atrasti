using System.Threading.Tasks;
using Atrasti.Data.Models;

namespace Atrasti.Data.Core
{
    public interface IUserDataRepository
    {
        Task UpdateUserData(UserData userData);
        
        Task UpdateRefreshToken(UserData userData);

        Task IncrementReferrals(int userId);

        Task DeleteUserData(int id);

        Task<UserData> FindUserDataById(int id);
    }
}