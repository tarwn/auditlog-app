using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuditLogApp.Common.Persistence
{
    public interface IPersistenceStore
    {
        ICustomerStore Customers { get; }
        ICustomerAuthenticationStore CustomerAuthentications { get; }
        IUserStore Users { get; }
        IUserAuthenticationStore UserAuthentications { get; }
        IUserSessionStore UserSessions { get; }
        IEventEntriesStore EventEntries { get; }
        IServerStore Server { get; }
        IViewStore Views { get; }

        Task RequireTransactionAsync(Func<Task> action);
    }
}
