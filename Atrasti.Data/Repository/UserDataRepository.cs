using System.Data.Common;
using System.Threading.Tasks;
using Atrasti.Data.Core;
using Atrasti.Data.Models;
using Dapper;

namespace Atrasti.Data.Repository
{
    internal class UserDataRepository : BaseRepository, IUserDataRepository
    {
        public UserDataRepository(ConnectionProvider connectionFactory)
            : base(connectionFactory)
        {
        }

        public Task UpdateUserData(UserData userData)
        {
            return WithConnection(async connection =>
            {
                await using DbTransaction transaction = await connection.BeginTransactionAsync();

                await transaction.Connection.ExecuteAsync(
                    "UPDATE userdata SET fcm_token = null WHERE fcm_token = @fcm_token LIMIT 1;",
                    new
                    {
                        fcm_token = userData.FcmToken
                    });
                
                await transaction.Connection.ExecuteAsync(
                    "UPDATE userdata SET fcm_token = @fcm_token WHERE user_id = @userId LIMIT 1;",
                    new
                    {
                        fcm_token = userData.FcmToken,
                        userId = userData.UserId
                    });

                await transaction.CommitAsync();
            });
        }
        
        public Task UpdateRefreshToken(UserData userData)
        {
            return WithConnection(connection => connection.ExecuteAsync(
                "UPDATE userdata SET refresh_token = @refresh_token WHERE user_id = @userId LIMIT 1;",
                new
                {
                    refresh_token = userData.RefreshToken,
                    userId = userData.UserId
                }));
        }

        public Task IncrementReferrals(int userId)
        {
            return WithConnection(connection => connection.ExecuteAsync(
                "UPDATE userdata SET referrals = referrals + 1 WHERE user_id = @userId LIMIT 1;",
                new
                {
                    userId
                }));
        }

        public Task DeleteUserData(int id)
        {
            return WithConnection(connection => connection.ExecuteAsync(
                "DELETE FROM userdata WHERE user_id = @id LIMIT 1",
                new
                {
                    id
                }));
        }

        public Task<UserData> FindUserDataById(int id)
        {
            const string query =
                @"SELECT user_id as userData_user_id, referrals as userData_referrals, 
                fcm_token as userData_fcm_token, 
                refresh_token as userData_refresh_token 
                FROM userdata WHERE user_id = @0 LIMIT 1";

            return WithConnection(connection => connection.SelectSingleAsync<UserData>(query, id));
        }
    }
}