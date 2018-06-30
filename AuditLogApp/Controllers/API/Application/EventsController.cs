using AuditLogApp.Common.Identity;
using AuditLogApp.Common.Persistence;
using AuditLogApp.Controllers.API.Application.Models.Events;
using AuditLogApp.Controllers.API.Public.Models.Events;
using AuditLogApp.Controllers.API.Public.Models.Events.Search;
using AuditLogApp.Membership;
using AuditLogApp.Models.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditLogApp.Controllers.API.Application
{
    [ApiVersion("1")]
    [Route("api/appln/v{version:apiVersion}/events")]
    [Authorize(Policy = "InteractiveAccessOnly")]
    public class EventsController : Controller
    {
        private IPersistenceStore _persistence;
        private IUserMembership _membership;

        public EventsController(IPersistenceStore persistence, IUserMembership membership)
        {
            _persistence = persistence;
            _membership = membership;
        }

        [HttpGet("{clientId}/{eventId}")]
        public async Task<IActionResult> GetEventAsync(string clientId, Guid eventId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiError(400, ModelState));
            }

            var customerId = _membership.GetCustomerId(User);
            var entryId = new EventEntryId(eventId);
            var entry = await _persistence.EventEntries.GetAsync(customerId, entryId);
            if (entry == null)
            {
                return NotFound(new ApiError(404, "Specified event not found"));
            }

            var recordedEntry = new RecordedEvent(entry, "/api/v1/events/");

            return Ok(recordedEntry);
        }

        [HttpGet("{clientId}")]
        public async Task<IActionResult> GetEventsAsync(string clientId, DateTime? fromDate = null, DateTime? throughDate = null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiError(400, ModelState));
            }

            var results = new ApplicationEventSearchResponse()
            {
                Id = new EventSearchId(DateTime.UtcNow, "GET", $"{Request.Path}{Request.QueryString}", "Event Search")
            };

            var customerId = _membership.GetCustomerId(User);
            if (clientId.Equals("all"))
                clientId = null;

            var entries = await _persistence.EventEntries.SearchAsync(customerId, clientId, fromDate, throughDate);
            results.Entries = entries.Select(e => new RecordedEvent(e, $"/api/appln/v1/events/{ e.Client_Id }/"))
                                     .ToList();

            return Ok(results);
        }


    }
}
