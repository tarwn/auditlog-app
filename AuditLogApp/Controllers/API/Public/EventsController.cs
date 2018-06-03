using AuditLogApp.Common.DTO;
using AuditLogApp.Common.Identity;
using AuditLogApp.Common.Persistence;
using AuditLogApp.Controllers.API.Public.Models;
using AuditLogApp.Controllers.API.Public.Models.Events;
using AuditLogApp.Documentation;
using AuditLogApp.Membership;
using AuditLogApp.Models.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditLogApp.Controllers.API.Public
{
    [IncludeInDocumentation]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class EventsController : Controller
    {
        private IPersistenceStore _persistence;
        private ICustomerMembership _membership;

        public EventsController(IPersistenceStore persistence, ICustomerMembership membership)
        {
            _persistence = persistence;
            _membership = membership;
        }
        
        [Authorize(Policy = "APIAccessOnly")]
        [HttpPost("")]
        [ProducesResponseType(typeof(EventAcceptedResponse), 200)]
        [ProducesResponseType(typeof(ApiError), 400)]
        [ProducesResponseType(typeof(ApiError), 404)]
        public async Task<IActionResult> SubmitEventAsync([FromBody] EventEntry[] entries)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiError(400, ModelState));
            }

            foreach (var entry in entries)
            {
                EnsureAllowedNullsAreRepresented(entry);
            }

            var customerId = _membership.GetCustomerId(User);
            var results = new EventAcceptedResponse();

            foreach (var entry in entries)
            {
                var dto = new EventEntryDTO(null, customerId, DateTime.UtcNow,
                entry.UUID,
                entry.Client.Id,
                entry.Client.Name,
                entry.Time.Value,   // nullable - ModelBinding should catch this, as it's annotated as Required - convert to UTC
                entry.Action,
                entry.Description,
                entry.URL,
                null,
                entry.Actor.UUID,
                entry.Actor.Name,
                entry.Actor.Email,
                entry.Context.Client.IPAddress,
                entry.Context.Client.BrowserAgent,
                entry.Context.Server.ServerId,
                entry.Context.Server.Version,
                entry.Target.Type,
                entry.Target.UUID,
                entry.Target.Label,
                entry.Target.URL);

                EventEntryId id = await _persistence.EventEntries.CreateAsync(dto);
                results.ReceivedEvents.Add(new ReceivedEventEntryId(id, entry.UUID));
            }

            return Ok(results);
        }

        private static void EnsureAllowedNullsAreRepresented(EventEntry entry)
        {
            if (entry.Context == null)
            {
                entry.Context = new EventEntryContext();
            }
            if (entry.Context.Client == null)
            {
                entry.Context.Client = new EventEntryContextClient();
            }
            if (entry.Context.Server == null)
            {
                entry.Context.Server = new EventEntryContextServer();
            }
            if (entry.Target == null)
            {
                entry.Target = new EventEntryTarget();
            }
        }
    }
}
