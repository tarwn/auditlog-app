using AuditLogApp.Common.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditLogApp.Persistence.SQLServer.Stores
{
    public class ServerStore : IServerStore
    {
        private DatabaseUtility _db;

        public ServerStore(DatabaseUtility dbUtility)
        {
            _db = dbUtility;
        }

        public async Task<string> GetLatestVersionAsync()
        {
            string sql = @"
                SELECT TOP 1 Name
                FROM dbo.UpdateTracking
                ORDER BY UpdateTrackingKey DESC;
            ";

            return await _db.Query(async (db) =>
            {
                var results = await db.FetchAsync<string>(sql);
                return results.SingleOrDefault();
            });
        }
    }
}
