using AuditLogApp.Common.Persistence;
using AuditLogApp.Persistence.SQLServer.Stores;
using System;
using System.Data.SqlClient;

namespace AuditLogApp.Persistence.SQLServer
{
    public class PersistenceStore : IPersistenceStore, IDisposable
    {
        private DatabaseUtility _dbUtility;

        public PersistenceStore(string connectionString)
        {
            var connection = new SqlConnection(connectionString);
            _dbUtility = new DatabaseUtility(connection);

            Customers = new CustomerStore(_dbUtility);
            Users = new UserStore(_dbUtility);
        }

        public ICustomerStore Customers { get; private set; }

        public IUserStore Users { get; private set; }

        public void Dispose()
        {
            _dbUtility.Dispose();
        }
    }
}
