using AuditLogApp.Common.Enums;
using AuditLogApp.Common.Identity;
using AuditLogApp.Membership.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuditLogApp.Membership
{
    public interface IUserMembership
    {
        MembershipOptions Options { get; }

        Task<RegisterResult> RegisterAsync(string username, string email, string password);
        Task<RegisterResult> RegisterExternalAsync(string username, string email, CredentialType credentialType, string identity, string displayName);
        Task<bool> IsUsernameAvailable(string username);
        Task<bool> IsAlreadyRegisteredAsync(CredentialType credentialType, string identity);

        Task<LoginResult> LoginAsync(string username, string password);
        Task<LoginResult> LoginExternalAsync(CredentialType credentialType, string identity);

        Task<bool> ValidateLoginAsync(ClaimsPrincipal principal);

        Task LogoutAsync();

        string GetSessionId(ClaimsPrincipal principal);
        Task<SessionDetails> GetSessionDetailsAsync(ClaimsPrincipal principal);
        Task<Dictionary<string, object>> DescribeUserForErrorAsync(ClaimsPrincipal principal);

        Task<RevocationDetails> RevokeAuthenticationAsync(UserId userId, UserAuthenticationId id);
    }
}
