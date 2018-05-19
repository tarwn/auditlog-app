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
        // for auth
        Task<UserDTO> GetAsync(UserId userId);

        Task<UserDTO> CreateAsync(UserDTO user);
        Task<UserDTO> GetAsync(CustomerId customerId, UserId id);
        Task<UserDTO> GetByUsernameAsync(string username);
        Task<bool> IsUsernameRegisteredAsync(string username);
    }
}
