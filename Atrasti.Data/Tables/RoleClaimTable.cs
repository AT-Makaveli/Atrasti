using Atrasti.Data.Core;
using Dapper;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Atrasti.Data.Tables
{
    internal class RoleClaimTable<TKey> : BaseRepository
    {
        internal RoleClaimTable(ConnectionProvider connectionFactory) : base(connectionFactory)
        {
        }

        internal Task<IList<Claim>> FindClaimsAsync(TKey key, CancellationToken cancellationToken)
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

        internal Task AddClaimAsync(TKey key, Claim claim, CancellationToken cancellationToken)
        {
            return WithConnection(connection =>
            {
                const string commandText = "INSERT INTO roleclaims(RoleId, ClaimType, ClaimValue) VALUES (@roleId, @claimType, @claimValue);";
                return connection.ExecuteAsync(commandText, new { roleId = key, claimType = claim.Type, claimValue = claim.Value });
            }, cancellationToken);
        }

        internal Task DeleteClaimAsync(TKey key, Claim claim, CancellationToken cancellationToken)
        {
            return WithConnection(connection =>
            {
                const string commandText = "DELETE FROM roleclaims WHERE RoleId = @roleId AND ClaimType = @claimType AND ClaimValue = @claimValue;";
                return connection.ExecuteAsync(commandText, new { roleId = key, claimType = claim.Type, claimValue = claim.Value });
            }, cancellationToken);
        }

        private class ClaimModel
        {
            public string ClaimType { get; set; }

            public string ClaimValue { get; set; }
        }
    }
}
