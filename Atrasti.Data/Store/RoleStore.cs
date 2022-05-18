using Atrasti.Data.Models;
using Atrasti.Data.Tables;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Atrasti.Data.Store
{
    internal class RoleStore<TKey, TRole, TUserRole, TRoleClaim>
        : RoleStoreBase<TRole, TKey, TUserRole, TRoleClaim>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
        where TUserRole : IdentityUserRole<TKey>, new()
        where TRoleClaim : IdentityRoleClaim<TKey>, new()
    {
        private readonly RoleTable<TKey, TRole> _roleTable;
        private readonly RoleClaimTable<TKey> _roleClaimTable;

        public override IQueryable<TRole> Roles => throw new NotImplementedException();


        /// <summary>
        /// Constructor that takes a MySQLDatabase as argument 
        /// </summary>
        /// <param name="database"></param>
        public RoleStore(IdentityErrorDescriber identityErrorDescriber, ConnectionProvider connectionProvider)
            : base(identityErrorDescriber)
        {
            _roleTable = new RoleTable<TKey, TRole>(connectionProvider);
            _roleClaimTable = new RoleClaimTable<TKey>(connectionProvider);
        }

        public override async Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var created = await _roleTable.InsertAsync(role, cancellationToken);
            return created ? IdentityResult.Success : IdentityResult.Failed(new IdentityError
            {
                Code = string.Empty,
                Description = $"Role '{role.Name}' could not be created."
            });
        }

        public override async Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var created = await _roleTable.UpdateAsync(role, cancellationToken);
            return created ? IdentityResult.Success : IdentityResult.Failed(new IdentityError
            {
                Code = string.Empty,
                Description = $"Role '{role.Name}' could not be updated."
            });
        }

        public override async Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var created = await _roleTable.DeleteAsync(role.Id, cancellationToken);
            return created ? IdentityResult.Success : IdentityResult.Failed(new IdentityError
            {
                Code = string.Empty,
                Description = $"Role '{role.Name}' could not be deleted."
            });
        }

        public override Task<TRole> FindByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return _roleTable.FindByIdAsync(id, cancellationToken);
        }

        public override Task<TRole> FindByNameAsync(string normalizedName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return _roleTable.FindByNameAsync(normalizedName, cancellationToken);
        }

        public override Task<IList<Claim>> GetClaimsAsync(TRole role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return _roleClaimTable.FindClaimsAsync(role.Id, cancellationToken);
        }

        public override Task AddClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return _roleClaimTable.AddClaimAsync(role.Id, claim, cancellationToken);
        }

        public override Task RemoveClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return _roleClaimTable.DeleteClaimAsync(role.Id, claim, cancellationToken);
        }
    }
}