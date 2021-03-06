﻿using AuditLogApp.Common.Enums;
using AuditLogApp.Common.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace AuditLogApp.Membership
{
    public class CustomerAPIAuthHandler : AuthenticationHandler<CustomerAPIAuthOptions>
    {
        private ICustomerMembership _membership;

        public CustomerAPIAuthHandler(ICustomerMembership membership, IOptionsMonitor<CustomerAPIAuthOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            _membership = membership;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Is this relevant to us?
            if (!Request.Headers.TryGetValue(Options.HTTPHeader, out var authValue))
            {
                return AuthenticateResult.NoResult();
            }

            // Is it a good pair?
            var actualAuthValue = authValue.FirstOrDefault();
            var apiValues = actualAuthValue.Split(':', 2);
            if (apiValues.Length != 2 || String.IsNullOrEmpty(apiValues[0]) || String.IsNullOrEmpty(apiValues[1]))
            {
                return AuthenticateResult.Fail($"Invalid authentication format, expected 'id:secret'");
            }


            var customerAuthId = CustomerAuthenticationId.FromString(apiValues[0]);
            var secret = apiValues[1];
            var principal = await _membership.GetOneTimeLoginAsync(customerAuthId, secret, CredentialType.CustomerAPIKey);
            if (principal == null)
            {
                return AuthenticateResult.Fail("Invalid authentication provided, access denied.");
            }

            var ticket = new AuthenticationTicket(principal, Options.AuthenticationScheme);
            return AuthenticateResult.Success(ticket);
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.Headers["WWW-Authenticate"] = $"Basic realm=\"{Options.WWWAuthenticateRealm}\", charset=\"UTF-8\"";
            await base.HandleChallengeAsync(properties);
        }

        protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            return base.HandleForbiddenAsync(properties);
        }
    }
}
