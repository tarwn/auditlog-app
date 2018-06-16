using AuditLogApp.Common.Persistence;
using AuditLogApp.Persistence.SQLServer.Stores;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Transactions;

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
            CustomerAuthentications = new CustomerAuthenticationStore(_dbUtility);
            EventEntries = new EventEntriesStore(_dbUtility);
            Users = new UserStore(_dbUtility);
            UserAuthentications = new UserAuthenticationStore(_dbUtility);
            UserSessions = new UserSessionStore(_dbUtility);
            Views = new ViewStore(_dbUtility);
        }


        public ICustomerStore Customers { get; private set; }

        public ICustomerAuthenticationStore CustomerAuthentications { get; private set; }

        public IEventEntriesStore EventEntries { get; private set; }

        public IUserStore Users { get; private set; }

        public IUserAuthenticationStore UserAuthentications { get; private set; }

        public IUserSessionStore UserSessions { get; private set; }

        public IViewStore Views { get; private set; }
        
        public void Dispose()
        {
            _dbUtility.Dispose();
        }

        public async Task RequireTransactionAsync(Func<Task> executeAsync)
        {
            // requires updated version of SQLClient - https://github.com/dotnet/corefx/issues/24282
            // using (var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            //{
            //    await action();
            //}

            await _dbUtility.WithTransaction(executeAsync);
        }
    }
}
