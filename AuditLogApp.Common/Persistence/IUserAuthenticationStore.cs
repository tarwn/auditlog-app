using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AuditLogApp.Common.DTO;
using AuditLogApp.Common.Enums;
using AuditLogApp.Common.Identity;

namespace AuditLogApp.Common.Persistence
{
    public interface IUserAuthenticationStore
    {
        Task<UserAuthenticationDTO> CreateAsync(UserAuthenticationDTO authMethod);
        Task<bool> IsIdentityRegisteredAsync(CredentialType credentialType, string secret);
        Task<UserAuthenticationDTO> GetAsync(UserAuthenticationId id);
        Task<List<UserAuthenticationDTO>> GetByUserAsync(CredentialType credentialType, UserId id);
        Task<UserDTO> GetBySecretAsync(CredentialType credentialType, string secret);
        Task UpdateAsync(UserAuthenticationDTO userAuth);
    }
}
