using AuditLogApp.Common.DTO;
using AuditLogApp.Common.Identity;
using AuditLogApp.Common.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditLogApp.Persistence.SQLServer.Stores
{
    public class UserStore : IUserStore
    {
        private DatabaseUtility _db;

        public UserStore(DatabaseUtility dbUtility)
        {
            _db = dbUtility;
        }

        public async Task<UserDTO> CreateAsync(UserDTO user)
        {
            var sqlParams = new
            {
                CustomerId = user.CustomerId.RawValue,
                user.DisplayName,
                user.EmailAddress,
                user.Username,
                user.IsEnabled//,
                //Created = DateTime.UtcNow
            };

            string sql = @";
                INSERT INTO dbo.Users(CustomerId, UserName, DisplayName, EmailAddress, IsEnabled)
                VALUES(@CustomerId, @Username, @DisplayName, @EmailAddress, @IsEnabled);

                SELECT U.Id,
                       U.CustomerId,
                       U.UserName,
                       U.DisplayName,
                       U.EmailAddress,
                       U.IsEnabled
                       -- PasswordResetKey,
                       -- PasswordResetGoodUntil, 
                FROM dbo.Users U 
                WHERE Id = SCOPE_IDENTITY();
            ";

            return await _db.QuerySingle(async (db) => 
            { 
                return await db.FetchAsync<UserDTO>(sql, sqlParams);
            });
        }

        public async Task<UserDTO> GetAsync(CustomerId customerId, UserId id)
        {
            var sqlParams = new
            {
                CustomerId = customerId.RawValue,
                UserId = id.RawValue
            };
            string sql = @"
                SELECT U.Id,
                       U.CustomerId,
                       U.UserName,
                       U.DisplayName,
                       U.EmailAddress,
                       U.IsEnabled
                       -- PasswordResetKey,
                       -- PasswordResetGoodUntil, 
                FROM dbo.Users U
                WHERE U.CustomerId = @CustomerId
                    AND U.Id = @UserId;
            ";

            return await _db.QuerySingleOrDefault(async (db) => 
            {
                return await db.FetchAsync<UserDTO>(sql, sqlParams);
            });
        }

        public async Task<UserDTO> GetAsync(UserId id)
        {
            var sqlParams = new
            {
                UserId = id.RawValue
            };
            string sql = @"
                SELECT U.Id,
                       U.CustomerId,
                       U.UserName,
                       U.DisplayName,
                       U.EmailAddress,
                       U.IsEnabled
                       -- PasswordResetKey,
                       -- PasswordResetGoodUntil, 
                FROM dbo.Users U
                WHERE U.Id = @UserId;
            ";

            return await _db.QuerySingleOrDefault(async (db) =>
            {
                return await db.FetchAsync<UserDTO>(sql, sqlParams);
            });
        }

        public async Task<UserDTO> GetByUsernameAsync(string username)
        {
            var sqlParams = new
            {
                Username = username
            };
            string sql = @"
                SELECT U.Id,
                       U.CustomerId,
                       U.UserName,
                       U.DisplayName,
                       U.EmailAddress,
                       U.IsEnabled
                       -- PasswordResetKey,
                       -- PasswordResetGoodUntil, 
                FROM dbo.Users U
                WHERE U.Username LIKE @Username;
            ";

            return await _db.QuerySingleOrDefault(async (db) =>
            {
                return await db.FetchAsync<UserDTO>(sql, sqlParams);
            });
        }

        public async Task<bool> IsUsernameRegisteredAsync(string username)
        {
            var user = await GetByUsernameAsync(username);
            return (user != null);
        }
    }
}
