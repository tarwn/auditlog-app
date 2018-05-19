using System;
using System.Collections.Generic;
using System.Text;

namespace AuditLogApp.Common.Persistence
{
    public interface IPersistenceStore
    {
        ICustomerStore Customers { get; }
        IUserStore Users { get; }
    }
}
