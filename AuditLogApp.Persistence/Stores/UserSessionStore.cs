using AuditLogApp.Common.DTO;
using AuditLogApp.Common.Identity;
using AuditLogApp.Common.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuditLogApp.Persistence.SQLServer.Stores
{
    public class UserSessionStore : IUserSessionStore
    {
        private DatabaseUtility _db;

        public UserSessionStore(DatabaseUtility dbUtility)
        {
            _db = dbUtility;
        }

        public async Task<UserSessionDTO> CreateAsync(UserId userId, DateTime utcNow)
        {
            var sqlParams = new
            {
                Id = Guid.NewGuid(),
                UserId = userId.RawValue,
                CreationTime = DateTime.UtcNow
            };

            string sql = @";
                INSERT INTO dbo.UserSessions(Id, UserId, CreationTime, LogoutTime)
                VALUES(@Id, @UserId, @CreationTime, NULL);

                SELECT Id,
                       UserId,
                       CreationTime,
                       LogoutTime
                FROM dbo.UserSessions
                WHERE Id = @Id;
            ";

            return await _db.QuerySingle(async (db) =>
            {
                return await db.FetchAsync<UserSessionDTO>(sql, sqlParams);
            });
        }

        public async Task<UserSessionDTO> GetAsync(UserSessionId id)
        {
            var sqlParams = new
            {
                Id = id
            };

            string sql = @";
                SELECT Id,
                       UserId,
                       CreationTime,
                       LogoutTime
                FROM dbo.UserSessions
                WHERE Id = @Id;
            ";

            return await _db.QuerySingle(async (db) =>
            {
                return await db.FetchAsync<UserSessionDTO>(sql, sqlParams);
            });
        }

        public async Task LogoutAsync(UserSessionId id, DateTime utcNow)
        {
            var sqlParams = new
            {
                Id = id,
                LogoutTime = DateTime.UtcNow
            };

            string sql = @";
                UPDATE dbo.UserSessions
                SET LogoutTime = @LogoutTime
                WHERE Id = @Id
                    AND LogoutTime IS NULL;
            ";

            await _db.Execute(async (db) =>
            {
                await db.ExecuteAsync(sql, sqlParams);
            });
        }
    }
}
