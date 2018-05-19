using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AuditLogApp.Common.DTO;
using AuditLogApp.Common.Identity;

namespace AuditLogApp.Common.Persistence
{
    public interface IUserStore
    {
        Task<UserDTO> GetAsync(CustomerId customerId, UserId id);
    }
}
