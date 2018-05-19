using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuditLogApp.Common.Persistence
{
    public interface IPersistenceStore
    {
        ICustomerStore Customers { get; }
        IUserStore Users { get; }
        IUserAuthenticationStore UserAuthentications { get; }
        IUserSessionStore UserSessions { get; }

        Task RequireTransactionAsync(Func<Task> action);
    }
}
