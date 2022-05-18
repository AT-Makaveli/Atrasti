using Atrasti.Data.Core;
using Dapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Atrasti.Data.Tables
{
    internal class UserLoginTable<TKey, TUserLogin>
        : BaseRepository
        where TUserLogin : IdentityUserLogin<TKey>
        where TKey : IEquatable<TKey>
    {
        internal UserLoginTable(ConnectionProvider connectionFactory) : base(connectionFactory)
        {
        }

        internal Task<IEnumerable<UserLoginInfo>> GetLoginsAsync(TKey userId, CancellationToken cancellationToken)
        {
            return WithConnection(async connection =>
            {
                const string commandText = "SELECT * " +
                                   "FROM userlogins " +
                                   "WHERE UserId = @UserId;";
                var userLogins = await connection.QueryAsync<UserLoginInfo>(commandText, new { UserId = userId });
                return userLogins;
            }, cancellationToken);
        }

        internal Task AddLoginAsync(TKey id, UserLoginInfo userLogin, CancellationToken cancellationToken)
        {
            return WithConnection(async connection =>
            {
                const string commandText = "INSERT INTO userlogins(UserId, LoginProvider, ProviderKey) VALUES (@userId, @loginProvider, @providerKey);";
                await connection.ExecuteAsync(commandText, new
                {
                    userId = id,
                    loginProvider = userLogin.LoginProvider,
                    providerKey = userLogin.ProviderKey
                });
            }, cancellationToken);
        }

        internal Task RemoveLoginAsync(TKey id, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            return WithConnection(async connection =>
            {
                const string commandText = "DELETE FROM userlogins WHERE LoginProvider = @loginProvider AND ProviderKey = @providerKey AND UserId = @userId;";
                await connection.ExecuteAsync(commandText, new
                {
                    userId = id,
                    loginProvider = loginProvider,
                    providerKey = providerKey
                });
            }, cancellationToken);
        }

        internal Task<TUserLogin> FindUserLoginAsync(TKey userId, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            return WithConnection(async connection =>
            {
                const string commandText = "SELECT * FROM userlogins WHERE UserId = @userId AND LoginProvider = @loginProvider AND ProviderKey = @providerKey LIMIT 1;";
                return await connection.QuerySingleAsync<TUserLogin>(commandText, new
                {
                    userId = userId,
                    loginProvider = loginProvider,
                    providerKey = providerKey
                });
            }, cancellationToken);
        }

        internal Task<TUserLogin> FindUserLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            return WithConnection(async connection =>
            {
                const string commandText = "SELECT * FROM userlogins LoginProvider = @loginProvider AND ProviderKey = @providerKey LIMIT 1;";
                return await connection.QuerySingleAsync<TUserLogin>(commandText, new
                {
                    loginProvider = loginProvider,
                    providerKey = providerKey
                });
            }, cancellationToken);
        }
    }
}