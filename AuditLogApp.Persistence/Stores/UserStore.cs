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
                       U.DisplayName
                FROM dbo.Users U
                WHERE U.CustomerId = @CustomerId
                    AND U.Id = @UserId;
            ";

            return await _db.Query(async (db) =>
            {
                var results = await db.FetchAsync<UserDTO>(sql, sqlParams);
                return results.SingleOrDefault();
            });
        }
    }
}
