using System.Collections.Generic;
using System.Threading.Tasks;
using Atrasti.Data.Core;
using Atrasti.Data.Models.Cache;

namespace Atrasti.Data.Repository
{
    internal class UserCacheRepository : BaseRepository, IUserCacheRepository
    {
        public UserCacheRepository(ConnectionProvider connectionProvider)
            : base(connectionProvider)
        {
        }

        public Task SaveSearchEntry(UserCache cacheEntry)
        {
            const string query = "INSERT INTO user_cache(RefId, Value, Type) VALUES(@0, @1, @2) ON DUPLICATE KEY UPDATE Value = @1";
            return WithConnection(connection =>
                connection.Insert(query, cacheEntry.RefId, cacheEntry.Value, cacheEntry.Type));
        }

        public Task<UserCache> GetUserCache(int userId, CacheType type)
        {
            const string query = "SELECT * FROM user_cache WHERE RefId = @0 AND Type = @1";
            return WithConnection(connection => connection.SelectSingleAsync<UserCache>(query, userId, type));
        }
    }
}