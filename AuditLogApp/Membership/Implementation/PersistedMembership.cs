using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AuditLogApp.Common.DTO;
using AuditLogApp.Common.Enums;
using AuditLogApp.Common.Identity;
using AuditLogApp.Common.Persistence;
using AuditLogApp.Membership.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace AuditLogApp.Membership.Implementation
{
    public class PersistedUserMembership : IUserMembership
    {
        private IHttpContextAccessor _context;
        private IPersistenceStore _persistence;

        public PersistedUserMembership(IHttpContextAccessor context, UserMembershipOptions options, IPersistenceStore persistence)
        {
            _context = context;
            _persistence = persistence;
            Options = options;
        }

        public UserMembershipOptions Options { get; private set; }

        // Register

        public async Task<RegisterResult> RegisterAsync(string username, string email, string password)
        {
            var user = new UserDTO(null, 1, username, username, email);
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            var authMethod = new UserAuthenticationDTO(null, null, CredentialType.PasswordHash, passwordHash, "Password", DateTime.UtcNow);

            try
            {
                await _persistence.RequireTransactionAsync(async () =>
                {
                    user = await _persistence.Users.CreateAsync(user);
                    authMethod.UserId = user.Id;
                    authMethod = await _persistence.UserAuthentications.CreateAsync(authMethod);
                });
            }
            catch (Exception exc)
            {
                //TODO reduce breadth of exception statement
                return RegisterResult.GetFailed("Username is already in use");
            }

            // add in option of an email activation step and use options to provide redirect url

            await SignInAsync(user);

            return RegisterResult.GetSuccess();
        }

        public async Task<RegisterResult> RegisterExternalAsync(string username, string email, CredentialType credentialType, string identity, string displayName)
        {
            var user = new UserDTO(null, 1, username, username, email);
            var authMethod = new UserAuthenticationDTO(null, null, credentialType, identity, displayName, DateTime.UtcNow);
            try
            {
                await _persistence.RequireTransactionAsync(async () =>
                {
                    user = await _persistence.Users.CreateAsync(user);
                    authMethod.UserId = user.Id;
                    authMethod = await _persistence.UserAuthentications.CreateAsync(authMethod);
                });
            }
            catch (Exception exc)
            {
                //TODO reduce breadth of exception statement
                return RegisterResult.GetFailed("Username is already in use");
            }

            // add in option of an email activation step and use options to provide redirect url

            await SignInAsync(user);

            return RegisterResult.GetSuccess();
        }

        public async Task<bool> IsAlreadyRegisteredAsync(CredentialType credentialType, string identity)
        {
            return await _persistence.UserAuthentications.IsIdentityRegisteredAsync(credentialType, identity);
        }

        public async Task<bool> IsUsernameAvailable(string username)
        {
            return await _persistence.Users.IsUsernameRegisteredAsync(username);
        }

        // Login

        public async Task<LoginResult> LoginAsync(string username, string password)
        {
            var user = await _persistence.Users.GetByUsernameAsync(username);
            if (user == null)
            {
                return LoginResult.GetFailed();
            }

            var auth = (await _persistence.UserAuthentications.GetByUserAsync(CredentialType.PasswordHash, user.Id))
                                                          .SingleOrDefault();
            if (!BCrypt.Net.BCrypt.Verify(password, auth.Secret))
            {
                return LoginResult.GetFailed();
            }

            // add validation that user is allowed to login

            // add in option of for multifactor and use options to provide redirect url

            await SignInAsync(user);

            return LoginResult.GetSuccess();
        }

        public async Task<LoginResult> LoginExternalAsync(CredentialType credentialType, string identity)
        {
            var user = await _persistence.UserAuthentications.GetBySecretAsync(credentialType, identity);
            if (user == null)
            {
                return LoginResult.GetFailed();
            }

            // add validation that user is allowed to login

            // add in option of for multifactor and use options to provide redirect url

            await SignInAsync(user);

            return LoginResult.GetSuccess();
        }

        private async Task SignInAsync(UserDTO user)
        {
            // key the login to a server-side session id to make it easy to invalidate later
            var session = await _persistence.UserSessions.CreateAsync(user.Id, DateTime.UtcNow);

            var identity = new ClaimsIdentity(Options.InteractiveAuthenticationType);
            AddUserIdClaim(identity, session.UserId.RawValue.ToString());
            AddSessionIdClaim(identity, session.Id.RawValue.ToString());
            await _context.HttpContext.SignInAsync(new ClaimsPrincipal(identity));
        }

        // Validate Login
        
        public async Task<bool> ValidateLoginAsync(ClaimsPrincipal principal)
        {
            var sessionId = GetSessionId(principal);
            if (sessionId == null)
            {
                return false;
            }

            var session = await _persistence.UserSessions.GetAsync(UserSessionId.FromString(sessionId));
            if (session.LogoutTime.HasValue)
            {
                return false;
            }

            // add in options like updating it with a last seen time, expiration, etc
            // add in options like IP Address roaming check

            return true;
        }

        // Logout

        public async Task LogoutAsync()
        {
            await _context.HttpContext.SignOutAsync();

            var sessionId = GetSessionId(_context.HttpContext.User);
            if (sessionId != null)
            {
                await _persistence.UserSessions.LogoutAsync(UserSessionId.FromString(sessionId), DateTime.UtcNow);
            }
        }

        public async Task<RevocationDetails> RevokeAuthenticationAsync(UserId userId, UserAuthenticationId id)
        {
            var userAuth = await _persistence.UserAuthentications.GetAsync(id);
            if (!userAuth.UserId.Equals(userId))
            {
                return RevocationDetails.GetFailed("Could not find specified account details");
            }

            if (userAuth.IsRevoked)
            {
                return RevocationDetails.GetFailed("Linked account has already been revoked");
            }

            userAuth.IsRevoked = true;
            userAuth.RevokeTime = DateTime.UtcNow;
            await _persistence.UserAuthentications.UpdateAsync(userAuth);

            return RevocationDetails.GetSuccess();
        }

        // Describe User / Session

        public async Task<Dictionary<string, object>> DescribeUserForErrorAsync(ClaimsPrincipal principal)
        {
            if (IsSession(principal))
            {
                var sessionDetails = await GetSessionDetailsAsync(principal);
                return new Dictionary<string, object>() {
                    { "Login Type", "Session (Interactive)" },
                    { "User.Id", sessionDetails.User.Id },
                    { "User.Username", sessionDetails.User.Username ?? "" },
                    { "User.EmailAddress", sessionDetails.User.EmailAddress ?? "" },
                    { "Session.Id", sessionDetails.Id },
                    { "Session.CreationTime", sessionDetails.CreationTime }
                };
            }
            else
            {
                return new Dictionary<string, object>() {
                    { "User Details", "Anonymous User" }
                };
            }
        }

        public async Task<SessionDetails> GetSessionDetailsAsync(ClaimsPrincipal principal)
        {
            var sessionId = GetSessionId(principal);
            if (sessionId == null)
            {
                return null;
            }

            var session = await _persistence.UserSessions.GetAsync(UserSessionId.FromString(sessionId));
            var user = await _persistence.Users.GetAsync(session.UserId);

            return new SessionDetails()
            {
                Id = session.Id,
                CreationTime = session.CreationTime,
                LogoutTime = session.LogoutTime,
                User = new UserDetails()
                {
                    Id = user.Id,
                    Username = user.Username,
                    EmailAddress = user.EmailAddress
                }
            };
        }

        #region Claims

        private void AddSessionIdClaim(ClaimsIdentity identity, string sessionId)
        {
            identity.AddClaim(new Claim("sessionId", sessionId));
        }

        public string GetSessionId(ClaimsPrincipal principal)
        {
            return principal.FindFirstValue("sessionId");
        }

        private bool IsSession(ClaimsPrincipal principal)
        {
            return principal.Identities.Any(i => i.IsAuthenticated && i.AuthenticationType == Options.InteractiveAuthenticationType)
                && GetSessionId(principal) != null;
        }

        private void AddUserIdClaim(ClaimsIdentity identity, string userId)
        {
            identity.AddClaim(new Claim("userId", userId));
        }

        private string GetUserId(ClaimsPrincipal principal)
        {
            return principal.FindFirstValue("userId");
        }

        private void AddUserAuthClaim(ClaimsIdentity identity, string userAuthId)
        {
            identity.AddClaim(new Claim("userAuthId", userAuthId));
        }

        #endregion
    }
}
