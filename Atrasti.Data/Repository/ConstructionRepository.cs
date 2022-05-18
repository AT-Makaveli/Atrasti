using System.Threading;
using System.Threading.Tasks;
using Atrasti.Data.Core;
using Dapper;

namespace Atrasti.Data.Repository
{
    internal class ConstructionRepository : BaseRepository, IConstructionRepository
    {
        public ConstructionRepository(ConnectionProvider connectionFactory) : base(connectionFactory)
        {
        }
        
        public Task<bool> SignUpEmailForNotificationAsync(string email, string continent)
        {
            return WithConnection(async connection =>
            {
                int rowsInserted = await connection.ExecuteAsync("INSERT IGNORE INTO construction_emails(`email`, `continent`) VALUES(@email, @continent);", new
                {
                    email, continent
                });
                return rowsInserted == 1;
            }, CancellationToken.None);
        }
    }
}