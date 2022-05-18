using Atrasti.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Atrasti.Data.Core
{
    public interface IUserRepository
    {
        Task<ICollection<AtrastiUser>> FindSetupUsersAsync(int[] ids);

        Task<AtrastiUser> FindSetupUserById(int id);

        Task<AtrastiUser> FindUserById(int id);

        Task<ICollection<AtrastiUser>> FindTop10Users();

        Task<ICollection<AtrastiUser>> SearchUsersByCompanyAsync(string[] query);

        Task<bool> FindByCompany(string company);

        Task<AtrastiUser> FindUserWithUserDataByRefreshToken(string refreshToken);

        Task<AtrastiUser> FindUserWithUserDataByEmail(string email);

        Task<IList<AtrastiUser>> FindUsersByIds(IEnumerable<int> userIds);
    }
}
