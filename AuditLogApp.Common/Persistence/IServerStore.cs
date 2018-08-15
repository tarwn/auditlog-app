using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuditLogApp.Common.Persistence
{
    public interface IServerStore
    {
        Task<string> GetLatestVersionAsync();
    }
}
