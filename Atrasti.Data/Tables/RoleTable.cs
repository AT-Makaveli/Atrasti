using Atrasti.Data.Core;
using Atrasti.Data.Models;
using Dapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Atrasti.Data.Tables
{
    internal class RoleTable<TKey, TRole> : 
        BaseRepository
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
    {
        internal RoleTable(ConnectionProvider connectionFactory) : base(connectionFactory)
        {
        }

        internal Task<bool> DeleteAsync(TKey roleId, CancellationToken cancellationToken)
        {
            const string commandText = "DELETE FROM Roles WHERE Id = @id";
            return WithConnection(async connection =>
            {
                var rowsDeleted = await connection.ExecuteAsync(commandText, new { id = roleId });
                return rowsDeleted == 1;
            }, cancellationToken);
        }

        internal Task<bool> InsertAsync(TRole role, CancellationToken cancellationToken)
        {
            const string commandText = "INSERT INTO Roles (Id, Name) VALUES (@id, @name)";
            return WithConnection(async connection =>
            {
                var rowsDeleted = await connection.ExecuteAsync(commandText, new { id = role.Id, name = role.Name });
                return rowsDeleted == 1;
            }, cancellationToken);
        }

        internal Task<bool> UpdateAsync(TRole role, CancellationToken cancellationToken)
        {
            return WithConnection(async connection =>
            {
                const string commandText = "UPDATE Roles SET Name = @name WHERE Id = @id";
                var rowsDeleted = await connection.ExecuteAsync(commandText, new { id = role.Id });
                return rowsDeleted == 1;
            }, cancellationToken);
        }

        internal Task<TRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            return WithConnection(async connection =>
            {
                const string commandText = "SELECT * FROM Roles WHERE Id = @id";
                return await connection.QuerySingleAsync<TRole>(commandText, new { id = roleId });
            }, cancellationToken);
        }

        internal Task<TRole> FindByNameAsync(string name, CancellationToken cancellationToken)
        {
            return WithConnection(async connection =>
            {
                const string commandText = "SELECT * FROM Roles WHERE Name = @roleName";
                return await connection.QuerySingleAsync<TRole>(commandText, new { roleName = name });
            }, cancellationToken);
        }
    }
}