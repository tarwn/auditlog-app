using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AuditLogApp.Common.DTO;
using AuditLogApp.Common.Identity;

namespace AuditLogApp.Common.Persistence
{
    public interface IUserSessionStore
    {
        Task<UserSessionDTO> CreateAsync(UserId id, DateTime utcNow);
        Task<UserSessionDTO> GetAsync(UserSessionId id);
        Task LogoutAsync(UserSessionId id, DateTime utcNow);
    }
}
