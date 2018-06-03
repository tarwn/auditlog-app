using AuditLogApp.Common.Enums;
using AuditLogApp.Common.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuditLogApp.Membership
{
    public interface ICustomerMembership
    {
        Task<ClaimsPrincipal> GetOneTimeLoginAsync(CustomerAuthenticationId id, string secret, CredentialType credentialType);
        CustomerId GetCustomerId(ClaimsPrincipal user);
    }
}
