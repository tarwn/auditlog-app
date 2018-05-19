using AuditLogApp.Common.DTO;
using AuditLogApp.Common.Enums;
using AuditLogApp.Common.Identity;
using AuditLogApp.Common.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuditLogApp.Persistence.SQLServer.Stores
{
    public class UserAuthenticationStore : IUserAuthenticationStore
    {
        private DatabaseUtility _db;

        public UserAuthenticationStore(DatabaseUtility dbUtility)
        {
            _db = dbUtility;
        }

        public async Task<UserAuthenticationDTO> CreateAsync(UserAuthenticationDTO authMethod)
        {
            var sqlParams = new
            {
                Id = Guid.NewGuid(),
                UserId = authMethod.UserId.RawValue,
                CredentialType = (int)authMethod.CredentialType,
                authMethod.Secret,
                authMethod.DisplayName,
                CreationTime = DateTime.UtcNow
            };
            string sql = @";
                INSERT INTO dbo.UserAuthenticationMethods(Id, UserId, CredentialType, Secret, DisplayName, CreationTime, IsRevoked)
                VALUES(@Id, @UserId, @CredentialType, @Secret, @DisplayName, @CreationTime, 0);

                SELECT Id,
                       UserId,
                       CredentialType,
                       Secret,
                       DisplayName,
                       CreationTime,
                       IsRevoked,
                       RevokeTime
                FROM dbo.UserAuthenticationMethods 
                WHERE Id = @Id;
            ";

            return await _db.QuerySingle(async (db) =>
            {
                return await db.FetchAsync<UserAuthenticationDTO>(sql, sqlParams);
            });
        }

        public async Task<List<UserAuthenticationDTO>> GetByUserAsync(CredentialType credentialType, UserId userId)
        {
            var sqlParams = new
            {
                UserId = userId.RawValue,
                CredentialType = (int)credentialType,
            };
            string sql = @";
                SELECT Id,
                       UserId,
                       CredentialType,
                       Secret,
                       DisplayName,
                       CreationTime,
                       IsRevoked,
                       RevokeTime
                FROM dbo.UserAuthenticationMethods 
                WHERE UserId = @UserId
                    And CredentialType = @CredentialType;
            ";

            return await _db.Query(async (db) =>
            {
                return await db.FetchAsync<UserAuthenticationDTO>(sql, sqlParams);
            });
        }

        public async Task<UserAuthenticationDTO> GetAsync(UserAuthenticationId id)
        {
            var sqlParams = new
            {
                Id = id.RawValue
            };
            string sql = @";
                SELECT Id,
                       UserId,
                       CredentialType,
                       Secret,
                       DisplayName,
                       CreationTime,
                       IsRevoked,
                       RevokeTime
                FROM dbo.UserAuthenticationMethods 
                WHERE Id = @Id;
            ";

            return await _db.QuerySingleOrDefault(async (db) =>
            {
                return await db.FetchAsync<UserAuthenticationDTO>(sql, sqlParams);
            });
        }

        public async Task<UserDTO> GetBySecretAsync(CredentialType credentialType, string secret)
        {
            var sqlParams = new
            {
                CredentialType = (int) credentialType,
                Secret = secret
            };
            string sql = @";
                SELECT U.Id,
                       U.CustomerId,
                       U.UserName,
                       U.DisplayName,
                       U.EmailAddress,
                       U.IsEnabled
                       -- PasswordResetKey,
                       -- PasswordResetGoodUntil, 
                FROM dbo.Users U 
                    INNER JOIN dbo.UserAuthenticationMethods UAM ON UAM.UserId = U.Id
                WHERE UAM.CredentialType = @CredentialType
                    AND UAM.Secret = @Secret;
            ";

            return await _db.QuerySingleOrDefault(async (db) =>
            {
                return await db.FetchAsync<UserDTO>(sql, sqlParams);
            });
        }

        public async Task<bool> IsIdentityRegisteredAsync(CredentialType credentialType, string secret)
        {
            var sqlParams = new
            {
                CredentialType = (int)credentialType,
                Secret = secret
            };
            string sql = @";
                IF EXISTS (SELECT 1 FROM dbo.UserAuthenticationMethods WHERE CredentialType = @CredentialType AND Secret = @Secret)
                    SELECT 1;
                ELSE
                    SELECT 0;
            ";

            return await _db.Query(async (db) =>
            {
                return await db.ExecuteScalarAsync<bool>(sql, sqlParams);
            });
        }

        public async Task UpdateAsync(UserAuthenticationDTO userAuth)
        {
            var sqlParams = new
            {
                Id = Guid.NewGuid(),
                userAuth.DisplayName,
                userAuth.IsRevoked,
                userAuth.RevokeTime
            };
            string sql = @";
                UPDATE dbo.UserAuthenticationMethods
                SET DisplayName = @DisplayName,
                    IsRevoked = @IsRevoked,
                    RevokeTime = @RevokeTime    
                WHERE Id = @Id;
            ";

            await _db.Execute(async (db) =>
            {
                await db.ExecuteAsync(sql, sqlParams);
            });
        }
    }
}
