using Atrasti.Data.Core;
using Atrasti.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dapper;

namespace Atrasti.Data.Repository
{
    internal class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(ConnectionProvider connectionFactory)
            : base(connectionFactory)
        {
        }

        public async Task<ICollection<AtrastiUser>> FindSetupUsersAsync(int[] ids)
        {
            if (ids.Length == 0)
                return new List<AtrastiUser>();

            StringBuilder idBuilder = new StringBuilder();
            for (int i = 0; i < ids.Length; i++)
            {
                if (i + 1 == ids.Length)
                {
                    idBuilder.Append("@").Append(i);
                    break;
                }

                idBuilder.Append("@").Append(i).Append(",");
            }

            return await WithConnection(async connection =>
            {
                string commandQuery = string.Format(
                    "SELECT u.*, c.*, ci.*, userData.user_id as userData_user_id, userData.referrals as userData_referrals FROM Users u JOIN companies c ON u.Id = c.RefId JOIN company_infos ci ON u.Id = ci.RefId JOIN userdata userData on userData.user_id = u.Id WHERE Id IN ({0}) AND u.CompanySetup = 1 AND u.CompanyInfoSetup = 1;",
                    idBuilder.ToString());
                var command = connection.CreateCommandWithIntArray(commandQuery, ids);
                var users = await command.SelectMultipleAsync<AtrastiUser>();

                return (ICollection<AtrastiUser>) users;
            }, CancellationToken.None);
        }

        public Task<ICollection<AtrastiUser>> FindTop10Users()
        {
            return WithConnection(async connection =>
            {
                var users = await connection.SelectMultipleAsync<AtrastiUser>(
                    "SELECT u.*, c.*, ci.* FROM Users u JOIN companies c ON u.Id = c.RefId JOIN company_infos ci ON u.Id = ci.RefId WHERE Id IN (SELECT CompanyId FROM products GROUP BY CompanyId ORDER BY COUNT(*) DESC) AND u.CompanySetup = 1 AND u.CompanyInfoSetup = 1;");

                return (ICollection<AtrastiUser>) users;
            }, CancellationToken.None);
        }

        public Task<AtrastiUser> FindSetupUserById(int id)
        {
            return WithConnection(
                connection =>
                {
                    return connection.SelectSingleAsync<AtrastiUser>(
                        "SELECT u.*, c.*, ci.*, userData.user_id as userData_user_id, userData.referrals as userData_referrals, userData.fcm_token as userData_fcm_token FROM Users u JOIN companies c ON u.Id = c.RefId JOIN company_infos ci ON u.Id = ci.RefId JOIN userdata userData on userData.user_id = u.Id WHERE Id = @0 AND u.CompanySetup = 1 AND u.CompanyInfoSetup = 1;",
                        id);
                }, CancellationToken.None);
        }

        public Task<AtrastiUser> FindUserById(int id)
        {
            return WithConnection(
                connection =>
                {
                    return connection.SelectSingleAsync<AtrastiUser>(
                        "SELECT u.*, c.*, ci.* FROM Users u JOIN companies c ON u.Id = c.RefId JOIN company_infos ci ON u.Id = ci.RefId WHERE Id = @0;",
                        id);
                }, CancellationToken.None);
        }

        public async Task<ICollection<AtrastiUser>> SearchUsersByCompanyAsync(string[] query)
        {
            if (query.Length == 0) return new List<AtrastiUser>();
            StringBuilder idBuilder = new StringBuilder();
            for (int i = 0; i < query.Length; i++)
            {
                if (i + 1 == query.Length)
                {
                    idBuilder.Append(" Company LIKE @").Append(i);
                    break;
                }

                idBuilder.Append(" Company LIKE @").Append(i).Append(" OR");
            }

            return await WithConnection(
                async connection =>
                {
                    string commandQuery = $"SELECT * FROM Users WHERE{idBuilder} AND CompanySetup = 1";
                    var command = connection.CreateCommandWithStringArray(commandQuery, query, true);
                    var users = await command.SelectMultipleAsync<AtrastiUser>();
                    return users;
                }, CancellationToken.None);
        }

        public Task<bool> FindByCompany(string company)
        {
            return WithConnection(async connection =>
            {
                var exists = connection.ExecuteScalar<bool>("SELECT COUNT(1) FROM Users WHERE Company=@id",
                    new {id = company});
                return exists;
            }, CancellationToken.None);
        }

        public Task<AtrastiUser> FindUserWithUserDataByRefreshToken(string refreshToken)
        {
            const string query =
                @"SELECT u.*, ud.user_id as userData_user_id, ud.referrals as userData_referrals, 
                    ud.fcm_token as userData_fcm_token, 
                    ud.refresh_token as userData_refresh_token 
                    FROM userdata ud JOIN Users u ON u.Id = ud.user_id WHERE ud.refresh_token = @0 LIMIT 1";
            return WithConnection(
                connection => { return connection.SelectSingleAsync<AtrastiUser>(query, refreshToken); },
                CancellationToken.None);
        }

        public Task<AtrastiUser> FindUserWithUserDataByEmail(string email)
        {
            const string query =
                @"SELECT u.*, ud.user_id as userData_user_id, ud.referrals as userData_referrals, 
                    ud.fcm_token as userData_fcm_token, 
                    ud.refresh_token as userData_refresh_token 
                    FROM userdata ud JOIN Users u ON u.Id = ud.user_id WHERE u.Email = @0 LIMIT 1";
            return WithConnection(
                connection => { return connection.SelectSingleAsync<AtrastiUser>(query, email); },
                CancellationToken.None);
        }

        public Task<IList<AtrastiUser>> FindUsersByIds(IEnumerable<int> userIds)
        {
            return WithConnection(async connection =>
            {
                IEnumerable<AtrastiUser> result = await connection.QueryAsync<AtrastiUser>(
                    "SELECT * FROM Users WHERE Id IN @ids", new
                    {
                        ids = userIds
                    });

                return result.ToList() as IList<AtrastiUser>;
            });
        }
    }
}