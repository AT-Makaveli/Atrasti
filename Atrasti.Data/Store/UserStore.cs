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
    internal class UserStore<TKey, TUser, TUserClaim, TUserLogin, TUserToken>
        : UserStoreBase<TUser, TKey, TUserClaim, TUserLogin, TUserToken>
        where TUser : IdentityUser<TKey>
        where TKey : IEquatable<TKey>
        where TUserClaim : IdentityUserClaim<TKey>, new()
        where TUserLogin : IdentityUserLogin<TKey>, new()
        where TUserToken : IdentityUserToken<TKey>, new()
    {
        private readonly UserTable<TKey, TUser> _userTable;
        private readonly UserClaimTable<TKey, TUser> _userClaimTable;
        private readonly UserLoginTable<TKey, TUserLogin> _userLoginTable;
        private readonly UserTokenTable<TKey, TUserToken> _userTokenTable;

        //TODO: Maybe try cache values?

        public UserStore(IdentityErrorDescriber describer, ConnectionProvider connectionProvider) : base(describer)
        {
            _userTable = new UserTable<TKey, TUser>(connectionProvider);
            _userClaimTable = new UserClaimTable<TKey, TUser>(connectionProvider);
            _userLoginTable = new UserLoginTable<TKey, TUserLogin>(connectionProvider);
            _userTokenTable = new UserTokenTable<TKey, TUserToken>(connectionProvider);
        }

        public override IQueryable<TUser> Users => throw new NotImplementedException();

        public override Task AddClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return _userClaimTable.AddClaimsAsync(user.Id, claims, cancellationToken);
        }

        public override Task AddLoginAsync(TUser user, UserLoginInfo login, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.ThrowIfNull(nameof(user));
            login.ThrowIfNull(nameof(login));
            return _userLoginTable.AddLoginAsync(user.Id, login, cancellationToken);
        }

        public override async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var created = await _userTable.InsertAsync(user, cancellationToken);
            return created ? IdentityResult.Success : IdentityResult.Failed(new IdentityError
            {
                Code = string.Empty,
                Description = $"User '{user.UserName}' could not be created."
            });
        }

        public override async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var created = await _userTable.DeleteAsync(user, cancellationToken);
            return created ? IdentityResult.Success : IdentityResult.Failed(new IdentityError
            {
                Code = string.Empty,
                Description = $"User '{user.UserName}' could not be deleted."
            });
        }

        public override Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return _userTable.GetUserByEmailAsync(normalizedEmail, cancellationToken);
        }

        public override Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return _userTable.GetUserByIdAsync(userId, cancellationToken);
        }

        public override Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return _userTable.GetUserByNameAsync(normalizedUserName, cancellationToken);
        }

        public override Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return _userClaimTable.GetClaimsAsync(user.Id, cancellationToken);
        }

        public override async Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var logins = await _userLoginTable.GetLoginsAsync(user.Id, cancellationToken);
            return logins.ToList();
        }

        public override async Task<IList<TUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var users = await _userClaimTable.GetUsersForClaimAsync(claim, cancellationToken);
            return users.ToList();
        }

        public override Task RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return _userClaimTable.RemoveClaimsAsync(user.Id, claims, cancellationToken);
        }

        public override Task RemoveLoginAsync(TUser user, string loginProvider, string providerKey, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return _userLoginTable.RemoveLoginAsync(user.Id, loginProvider, providerKey, cancellationToken);
        }

        public override Task ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return _userClaimTable.ReplaceClaimAsync(user.Id, claim, newClaim, cancellationToken);
        }

        public override async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var created = await _userTable.UpdateAsync(user, cancellationToken);
            return created ? IdentityResult.Success : IdentityResult.Failed(new IdentityError
            {
                Code = string.Empty,
                Description = $"User '{user.UserName}' could not be updated."
            });
        }

        protected override Task AddUserTokenAsync(TUserToken token)
        {
            ThrowIfDisposed();
            return _userTokenTable.AddUserTokenAsync(token, CancellationToken.None);
        }

        protected override Task<TUserToken> FindTokenAsync(TUser user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return _userTokenTable.FindTokenAsync(user.Id, loginProvider, name, cancellationToken);
        }

        protected override Task<TUser> FindUserAsync(TKey userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return _userTable.GetUserByIdAsync(userId.ToString(), cancellationToken);
        }

        protected override Task<TUserLogin> FindUserLoginAsync(TKey userId, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return _userLoginTable.FindUserLoginAsync(userId, loginProvider, providerKey, cancellationToken);
        }

        protected override Task<TUserLogin> FindUserLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return _userLoginTable.FindUserLoginAsync(loginProvider, providerKey, cancellationToken);
        }

        protected override Task RemoveUserTokenAsync(TUserToken token)
        {
            ThrowIfDisposed();
            return _userTokenTable.DeleteTokenAsync(token, CancellationToken.None);
        }
    }
}