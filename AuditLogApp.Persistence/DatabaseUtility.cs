using AuditLogApp.Common.Identity;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AuditLogApp.Persistence.SQLServer
{
    public class DatabaseUtility : IDisposable
    {
        private SqlConnection _connection;
        private RetryPolicy<SqlDatabaseTransientErrorDetectionStrategy> _retryPolicy;
        private static object _lock = new object();

        public DatabaseUtility(SqlConnection connection)
        {
            _connection = connection;
            _retryPolicy = new RetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>(3, TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(100));

            lock (_lock)
            {
                if (AsyncPoco.Mappers.GetMapper(typeof(CustomerId)) is AsyncPoco.StandardMapper)
                {
                    AsyncPoco.Mappers.Register(Assembly.GetAssembly(typeof(IIdentity)), new IdentityMapper());
                }
            }
        }

        public async Task<T> Query<T>(Func<AsyncPoco.Database, Task<T>> query)
        {
            T result = default(T);

            await _retryPolicy.ExecuteAsync(async () =>
            {
                if (_connection.State != System.Data.ConnectionState.Open)
                {
                    await _connection.OpenAsync();
                }

                using (var db = new AsyncPoco.Database(_connection))
                {
                    result = await query(db);
                }
            });

            return result;
        }

        public async Task Execute(Func<AsyncPoco.Database, Task> action)
        {
            await _retryPolicy.ExecuteAsync(async () =>
            {
                if (_connection.State != System.Data.ConnectionState.Open)
                {
                    await _connection.OpenAsync();
                }

                using (var db = new AsyncPoco.Database(_connection))
                {
                    await action(db);
                }
            });
        }

        public void Dispose()
        {
            if (_connection.State == System.Data.ConnectionState.Open)
            {
                _connection.Close();
            }
            _connection.Dispose();
        }
    }
}
