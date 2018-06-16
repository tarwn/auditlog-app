using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AuditLogApp.Common.DTO;
using AuditLogApp.Common.Enums;
using AuditLogApp.Common.Identity;
using AuditLogApp.Common.Persistence;
using AuditLogApp.Controllers.API.Application.Models;
using AuditLogApp.Membership;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuditLogApp.Controllers.API.Application
{
    [ApiVersion("1")]
    [Route("api/appln/v{version:apiVersion}/configuration")]
    [Authorize(Policy = "InteractiveAccessOnly")]
    public class ConfigurationController : Controller
    {
        private IPersistenceStore _persistence;
        private IUserMembership _membership;

        public ConfigurationController(IPersistenceStore persistence, IUserMembership membership)
        {
            _persistence = persistence;
            _membership = membership;
        }

        [HttpGet("apiKeys")]
        public async Task<IActionResult> GetAPIKeysAsync()
        {
            // TODO - improve on this, we already load these details during auth validation
            var session = await _membership.GetSessionDetailsAsync(User);
            var user = await _persistence.Users.GetAsync(session.User.Id);
            var methods = await _persistence.CustomerAuthentications.GetByCredentialTypeAsync(user.CustomerId, CredentialType.CustomerAPIKey);
            var apikeys = methods.Select(m => new APIKeyModel(m));

            return Ok(apikeys);
        }
        
        [HttpPost("apiKeys/create")]
        public async Task<IActionResult> CreateAPIKeyAsync([FromBody] CreateAPIKeyModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO - improve on this, we already load these details during auth validation
            var session = await _membership.GetSessionDetailsAsync(User);
            var user = await _persistence.Users.GetAsync(session.User.Id);

            var secret = GenerateAPIKey();
            var newKey = new CustomerAuthenticationDTO(null, user.CustomerId, CredentialType.CustomerAPIKey, secret, model.DisplayName, DateTime.UtcNow, user.Id);
            newKey = await _persistence.CustomerAuthentications.CreateAsync(newKey);

            // TODO - log creation to the audit log

            return Ok(newKey);
        }
        
        [HttpPost("apiKeys/{id}/revoke")]
        public async Task<IActionResult> CreateAPIKeyAsync(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO - improve on this, we already load these details during auth validation
            var session = await _membership.GetSessionDetailsAsync(User);
            var user = await _persistence.Users.GetAsync(session.User.Id);

            var key = await _persistence.CustomerAuthentications.GetAsync(user.CustomerId, new CustomerAuthenticationId(id));
            if (key == null)
            {
                return NotFound();
            }

            key.IsRevoked = true;
            key.RevokeTime = DateTime.UtcNow;
            key = await _persistence.CustomerAuthentications.UpdateAsync(key);

            // TODO - log creation to the audit log

            return Ok(key);
        }

        public static string GenerateAPIKey()
        {
            var key = new byte[32];
            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(key);
            }
            return Convert.ToBase64String(key);
        }
    }
}