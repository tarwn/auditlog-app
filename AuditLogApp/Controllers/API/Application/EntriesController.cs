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
    [Route("api/appln/v{version:apiVersion}/entries")]
    [Authorize(Policy = "InteractiveAccessOnly")]
    public class EntriesController : Controller
    {
        private IPersistenceStore _persistence;
        private IUserMembership _membership;

        public EntriesController(IPersistenceStore persistence, IUserMembership membership)
        {
            _persistence = persistence;
            _membership = membership;
        }

        [HttpGet("{clientId}/{entryId}")]
        public async Task<IActionResult> GetEventAsync(string clientId, Guid entryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiError(400, ModelState));
            }

            var customerId = _membership.GetCustomerId(User);
            var eventEntryId = new EventEntryId(entryId);
            var entry = await _persistence.EventEntries.GetAsync(customerId, eventEntryId);
            if (entry == null)
            {
                return NotFound(new ApiError(404, "Specified event not found"));
            }

            var recordedEntry = new RecordedEvent(entry, "/api/v1/entries/");

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
                Id = new EventSearchId(DateTime.UtcNow, "GET", $"{Request.Path}{Request.QueryString}", "Audit Log Search")
            };

            var customerId = _membership.GetCustomerId(User);
            if (clientId.Equals("all"))
                clientId = null;

            var entries = await _persistence.EventEntries.SearchAsync(customerId, clientId, fromDate, throughDate);
            results.Entries = entries.Select(e => new RecordedEvent(e, $"/api/appln/v1/entries/{ e.Client_Id }/"))
                                     .ToList();

            return Ok(results);
        }


    }
}
