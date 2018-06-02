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
    public class CustomerStore : ICustomerStore
    {
        private DatabaseUtility _db;

        public CustomerStore(DatabaseUtility dbUtility)
        {
            _db = dbUtility;
        }

        public async Task<CustomerDTO> CreateAsync(CustomerDTO customer)
        {
            var sqlParams = new
            {
                customer.DisplayName,
                CreationTime = DateTime.UtcNow
            };

            string sql = @";
                INSERT INTO dbo.Customers(DisplayName, CreationTime)
                VALUES(@DisplayName, @CreationTime);

                SELECT C.Id,
                        C.DisplayName,
                        C.CreationTime
                FROM dbo.Customers C
                WHERE Id = SCOPE_IDENTITY();
            ";

            return await _db.QuerySingle(async (db) =>
            {
                return await db.FetchAsync<CustomerDTO>(sql, sqlParams);
            });
            
        }

        public async Task<CustomerDTO> GetAsync(CustomerId id)
        {
            var sqlParams = new
            {
                CustomerId = id.RawValue
            };
            string sql = @"
                SELECT C.Id,
                       C.DisplayName
                FROM dbo.Customers C
                WHERE Id = @CustomerId;
            ";

            return await _db.Query(async (db) =>
            {
                var results = await db.FetchAsync<CustomerDTO>(sql, sqlParams);
                return results.SingleOrDefault();
            });
        }
    }
}
