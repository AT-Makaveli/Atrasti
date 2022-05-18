using Atrasti.Data.Core;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Atrasti.Data.Tables
{
    internal class UserClaimTable<TKey, TUser> : BaseRepository
    {
        internal UserClaimTable(ConnectionProvider connectionFactory) : base(connectionFactory)
        {
        }


        internal Task AddClaimsAsync(TKey key, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            return WithConnection(async connection =>
            {
                using IDbTransaction transaction = await connection.BeginTransactionAsync(cancellationToken);
                const string commandText = "INSERT INTO roleclaims(RoleId, ClaimType, ClaimValue) VALUES (@roleId, @claimType, @claimValue);";
                foreach (Claim claim in claims)
                    await connection.ExecuteAsync(commandText, new { roleId = key, claimType = claim.Type, claimValue = claim.Value }, transaction);
                transaction.Commit();
            }, cancellationToken);
        }

        internal Task<IList<Claim>> GetClaimsAsync(TKey key, CancellationToken cancellationToken)
        {
            return WithConnection(async connection =>
            {
                const string commandText = "SELECT ClaimType, ClaimValue FROM roleclaims WHERE RoleId = @id;";
                IEnumerable<ClaimModel> dbClaims = await connection.QueryAsync<ClaimModel>(commandText, new { id = key });
                IList<Claim> claims = new List<Claim>();
                foreach (ClaimModel claim in dbClaims)
                {
                    claims.Add(new Claim(claim.ClaimType, claim.ClaimValue));
                }

                return claims;
            }, cancellationToken);
        }

        internal Task RemoveClaimsAsync(TKey key, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            return WithConnection(async connection =>
            {
                using IDbTransaction transaction = await connection.BeginTransactionAsync(cancellationToken);
                const string commandText = "DELETE FROM userclaims WHERE UserId = @userId AND ClaimType = @claimType AND ClaimValue = @claimValue;";
                foreach (Claim claim in claims)
                    await connection.ExecuteAsync(commandText, new { userId = key, claimType = claim.Type, claimValue = claim.Value }, transaction);
                transaction.Commit();
            }, cancellationToken);
        }

        internal Task ReplaceClaimAsync(TKey key, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            return WithConnection(async connection =>
            {
                const string commandText = "UPDATE userclaims SET ClaimType = @newClaimType, ClaimValue = @newClaimValue WHERE ClaimType = @oldClaimType AND ClaimValue = @oldClaimValue AND UserId = @userId LIMIT 1;";
                await connection.ExecuteAsync(commandText, new
                {
                    newClaimType = newClaim.Type,
                    newClaimValue = newClaim.Value,
                    oldClaimType = claim.Type,
                    oldClaimValue = claim.Value,
                    userId = key
                });
            }, cancellationToken);
        }

        internal Task<IEnumerable<TUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken = default)
        {
            return WithConnection(async connection =>
            {
                const string sql = "SELECT * " +
                       "FROM users AS u " +
                       "INNER JOIN userclaims AS uc ON u.Id = uc.UserId " +
                       "WHERE uc.ClaimType = @ClaimType AND uc.ClaimValue = @ClaimValue;";
                IEnumerable<TUser> users = await connection.QueryAsync<TUser>(sql, new
                {
                    ClaimType = claim.Type,
                    ClaimValue = claim.Value
                });

                return users;
            }, cancellationToken);
        }

        private class ClaimModel
        {
            public string ClaimType { get; set; }

            public string ClaimValue { get; set; }
        }
    }
}
