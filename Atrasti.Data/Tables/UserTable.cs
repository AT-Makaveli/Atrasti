using Atrasti.Data.Core;
using Atrasti.Data.Models;
using Dapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Atrasti.Data.Tables
{
    internal class UserTable<TKey, TUser> :
        BaseRepository
        where TUser : IdentityUser<TKey>
        where TKey : IEquatable<TKey>
    {
        public UserTable(ConnectionProvider connectionFactory) : base(connectionFactory)
        {
        }

        /// <summary>
        /// Returns the user's name given a user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<string> GetUserNameAsync(string userId, CancellationToken cancellationToken)
        {
            return WithConnection(async connection =>
            {
                const string commandText = "SELECT Name FROM Users WHERE Id = @id";
                return await connection.ExecuteScalarAsync<string>(commandText, new {id = userId});
            }, cancellationToken);
        }

        /// <summary>
        /// Returns a User ID given a user name
        /// </summary>
        /// <param name="userName">The user's name</param>
        /// <returns></returns>
        public Task<TKey> GetUserIdAsync(string userName, CancellationToken cancellationToken)
        {
            return WithConnection(async connection =>
            {
                const string commandText = "SELECT Id FROM Users WHERE UserName = @name";
                return await connection.ExecuteScalarAsync<TKey>(commandText, new {name = userName});
            }, cancellationToken);
        }

        /// <summary>
        /// Returns an TUser given the user's id
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public Task<TUser> GetUserByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return WithConnection(connection =>
            {
                const string commandText = "SELECT * FROM Users WHERE Id = @id";
                return connection.QuerySingleOrDefaultAsync<TUser>(commandText, new {id = userId});
            }, cancellationToken);
        }

        /// <summary>
        /// Returns TUser instance given a user name
        /// </summary>
        /// <param name="userName">User's name</param>
        /// <returns></returns>
        public Task<TUser> GetUserByNameAsync(string userName, CancellationToken cancellationToken)
        {
            return WithConnection(connection =>
            {
                const string commandText = "SELECT * FROM Users WHERE UserName = @name";
                return connection.QuerySingleOrDefaultAsync<TUser>(commandText, new {name = userName});
            }, cancellationToken);
        }

        public Task<TUser> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return WithConnection(connection =>
            {
                const string commandText = "SELECT * FROM Users WHERE Email = @email";
                return connection.QuerySingleOrDefaultAsync<TUser>(commandText, new {email});
            }, cancellationToken);
        }

        /// <summary>
        /// Return the user's password hash
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public Task<string> GetPasswordHashAsync(TKey userId, CancellationToken cancellationToken)
        {
            return WithConnection(async connection =>
            {
                const string commandText = "SELECT PasswordHash FROM Users WHERE Id = @id";
                return await connection.ExecuteScalarAsync<string>(commandText, new {id = userId});
            }, cancellationToken);
        }

        /// <summary>
        /// Sets the user's password hash
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
        public Task<bool> SetPasswordHashAsync(TKey userId, string passwordHash, CancellationToken cancellationToken)
        {
            return WithConnection(async connection =>
            {
                const string commandText = "UPDATE Users SET PasswordHash = @pwdHash WHERE Id = @id";
                var rowsAffected =
                    await connection.ExecuteAsync(commandText, new {id = userId, pwdHash = passwordHash});
                return rowsAffected == 1;
            }, cancellationToken);
        }

        /// <summary>
        /// Returns the user's security stamp
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<string> GetSecurityStampAsync(TKey userId, CancellationToken cancellationToken)
        {
            return WithConnection(async connection =>
            {
                const string commandText = "SELECT SecurityStamp FROM Users WHERE Id = @id";
                return await connection.ExecuteScalarAsync<string>(commandText, new {id = userId});
            }, cancellationToken);
        }

        /// <summary>
        /// Inserts a new user in the Users table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> InsertAsync(TUser user, CancellationToken cancellationToken)
        {
            AtrastiUser atrastiUser = user as AtrastiUser;
            return WithConnection(async connection =>
            {
                const string commandText =
                    "INSERT INTO Users (UserName, PasswordHash, SecurityStamp,Email,EmailConfirmed," +
                    "PhoneNumber,PhoneNumberConfirmed, AccessFailedCount,LockoutEnabled,LockoutEndDateUtc,TwoFactorEnabled,Company,FirstName,LastName,CompanyLogo, Referrer) " +
                    "VALUES (@name, @pwdHash, @SecStamp,@email,@emailconfirmed,@phonenumber,@phonenumberconfirmed," +
                    "@accesscount,@lockoutenabled,@lockoutenddate,@twofactorenabled,@company,@firstName,@lastName,@logo,@referrer)";
                var rowsAffected = await connection.ExecuteAsync(commandText,
                    new
                    {
                        name = user.UserName,
                        pwdHash = user.PasswordHash,
                        SecStamp = user.SecurityStamp,
                        email = user.Email,
                        emailconfirmed = user.EmailConfirmed,
                        phonenumber = user.PhoneNumber,
                        phonenumberconfirmed = user.PhoneNumberConfirmed,
                        accesscount = user.AccessFailedCount,
                        lockoutenabled = user.LockoutEnabled,
                        lockoutenddate = user.LockoutEnd != null
                            ? user.LockoutEnd.Value.DateTime
                            : DateTimeOffset.Now.DateTime,
                        twofactorenabled = user.TwoFactorEnabled,
                        company = atrastiUser.Company,
                        firstName = atrastiUser.FirstName,
                        lastName = atrastiUser.LastName,
                        logo = atrastiUser.CompanyLogo,
                        referrer = atrastiUser.Referrer
                    });
                if (rowsAffected == 1)
                {
                    user.Id = await GetUserIdAsync(user.UserName, cancellationToken);
                    await connection.ExecuteAsync("INSERT INTO userdata(user_id) VALUES (@userId)", new
                    {
                        userId = user.Id
                    });
                }

                return rowsAffected == 1;
            }, cancellationToken);
        }

        /// <summary>
        /// Deletes a user from the Users table
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        private Task<bool> DeleteAsync(TKey userId, CancellationToken cancellationToken)
        {
            return WithConnection(async connection =>
            {
                const string commandText = "DELETE FROM Users WHERE Id = @userId";
                var rowsAffected = await connection.ExecuteAsync(commandText, new {userId = userId});
                return rowsAffected == 1;
            }, cancellationToken);
        }

        /// <summary>
        /// Deletes a user from the Users table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> DeleteAsync(TUser user, CancellationToken cancellationToken)
        {
            return DeleteAsync(user.Id, cancellationToken);
        }

        /// <summary>
        /// Updates a user in the Users table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> UpdateAsync(TUser user, CancellationToken cancellationToken)
        {
            AtrastiUser atrastiUser = user as AtrastiUser;
            return WithConnection(async connection =>
            {
                const string commandText =
                    @"Update Users set UserName = @userName, PasswordHash = @pwdHash, SecurityStamp = @secStamp, 
                Email=@email, EmailConfirmed=@emailconfirmed, PhoneNumber=@phonenumber, PhoneNumberConfirmed=@phonenumberconfirmed,
                AccessFailedCount=@accesscount, LockoutEnabled=@lockoutenabled, LockoutEndDateUtc=@lockoutenddate, TwoFactorEnabled=@twofactorenabled, FirstName = @firstName, LastName = @lastName,
                CompanySetup = @companySetup, CompanyInfoSetup = @companyInfoSetup, CompanyLogo = @logo
                WHERE Id = @userId";

                var rowsAffected = await connection.ExecuteAsync(commandText,
                    new
                    {
                        userName = user.UserName,
                        userId = user.Id,
                        pwdHash = user.PasswordHash,
                        SecStamp = user.SecurityStamp,
                        email = user.Email,
                        emailconfirmed = user.EmailConfirmed,
                        phonenumber = user.PhoneNumber,
                        phonenumberconfirmed = user.PhoneNumberConfirmed,
                        accesscount = user.AccessFailedCount,
                        lockoutenabled = user.LockoutEnabled,
                        lockoutenddate = user.LockoutEnd != null
                            ? user.LockoutEnd.Value.ToString("yyyy-MM-dd HH:mm:ss")
                            : DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        twofactorenabled = user.TwoFactorEnabled,
                        firstName = atrastiUser.FirstName,
                        lastName = atrastiUser.LastName,
                        companySetup = atrastiUser.CompanySetup,
                        companyInfoSetup = atrastiUser.CompanyInfoSetup,
                        logo = atrastiUser.CompanyLogo
                    });
                return rowsAffected == 1;
            }, cancellationToken);
        }
    }
}