using System.Collections.Generic;
using System.Threading.Tasks;
using Atrasti.Data.Models.Cache;

namespace Atrasti.Data.Core
{
    public interface IUserCacheRepository
    {
        Task SaveSearchEntry(UserCache cacheEntry);

        Task<UserCache> GetUserCache(int userId, CacheType type);
    }
}