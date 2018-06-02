using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AuditLogApp.Common.DTO;
using AuditLogApp.Common.Enums;
using AuditLogApp.Common.Identity;

namespace AuditLogApp.Common.Persistence
{
    public interface ICustomerAuthenticationStore
    {
        Task<CustomerAuthenticationDTO> CreateAsync(CustomerAuthenticationDTO authMethod);
        Task<CustomerAuthenticationDTO> GetAsync(CustomerId customerId, CustomerAuthenticationId id);
        Task<CustomerAuthenticationDTO> GetAsync(CustomerAuthenticationId id);
        Task<List<CustomerAuthenticationDTO>> GetByCredentialTypeAsync(CustomerId customerId, CredentialType credentialType);
        Task<CustomerAuthenticationDTO> UpdateAsync(CustomerAuthenticationDTO authMethod);
    }
}
