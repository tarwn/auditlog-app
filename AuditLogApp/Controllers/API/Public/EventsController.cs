using AuditLogApp.Controllers.API.Public.Models;
using AuditLogApp.Documentation;
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
        [Authorize(Policy = "APIAccessOnly")]
        [HttpPost("")]
        [ProducesResponseType(typeof(EventAcceptedModel), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(ApiError), 404)]
        public async Task<IActionResult> SubmitEventAsync([FromBody] EventEntry entry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // convert from model to DTO
            // persist user + event
            
            return Ok(new EventAcceptedModel() {
                EventId = Guid.Empty
            });
        }
    }
}
