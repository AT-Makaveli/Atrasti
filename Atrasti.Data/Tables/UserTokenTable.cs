using Atrasti.Data.Core;
using Dapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Atrasti.Data.Tables
{
    internal class UserTokenTable<TKey, TUserToken>
        : BaseRepository
        where TKey : IEquatable<TKey>
        where TUserToken : IdentityUserToken<TKey>
    {
        internal UserTokenTable(ConnectionProvider connectionFactory) : base(connectionFactory)
        {
        }

        internal Task AddUserTokenAsync(TUserToken userToken, CancellationToken cancellationToken)
        {
            return WithConnection(connection =>
            {
                const string commandText = "INSERT INTO usertokens(UserId, LoginProvider, Name, Value) VALUES (@userId, @loginProvider, @name, @value);";
                return connection.ExecuteAsync(commandText, new
                {
                    userId = userToken.UserId,
                    loginProvider = userToken.LoginProvider,
                    name = userToken.Name,
                    value = userToken.Value
                });
            }, cancellationToken);
        }


        internal Task<TUserToken> FindTokenAsync(TKey id, string loginProvider, string name, CancellationToken cancellationToken)
        {
            return WithConnection(async connection =>
            {
                const string commandText = "SELECT * FROM usertokens WHERE UserId = @userId AND LoginProvider = @loginProvider AND @name LIMIT 1;";
                return await connection.QuerySingleAsync<TUserToken>(commandText, new
                {
                    userId = id,
                    loginProvider = loginProvider,
                    name = name
                });
            }, cancellationToken);
        }

        internal Task DeleteTokenAsync(TUserToken userToken, CancellationToken cancellationToken)
        {
            return WithConnection(connection =>
            {
                const string commandText = "DELETE FROM usertokens WHERE UserId = @userId;";
                return connection.ExecuteAsync(commandText, new { id = userToken.UserId });
            }, cancellationToken);
        }
    }
}
