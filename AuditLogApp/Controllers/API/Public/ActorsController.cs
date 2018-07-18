using AuditLogApp.Common.DTO;
using AuditLogApp.Common.Identity;
using AuditLogApp.Common.Persistence;
using AuditLogApp.Controllers.API.Public.Models.Actors;
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
    public class ActorsController : Controller
    {
        private IPersistenceStore _persistence;
        private ICustomerMembership _membership;

        public ActorsController(IPersistenceStore persistence, ICustomerMembership membership)
        {
            _persistence = persistence;
            _membership = membership;
        }

        /// <summary>
        /// Query to see what User information is in your AuditLog
        /// for the given User (by UUID).
        /// </summary>
        /// <param name="uuid">Your UUID for the given user</param>
        [Authorize(Policy = "APIAccessOnly")]
        [HttpGet("{uuid}")]
        [ProducesResponseType(typeof(ActorQueryResponse), 200)]
        public async Task<IActionResult> GetAsync(string uuid)
        {
            var customerId = _membership.GetCustomerId(User);
            var actors = await _persistence.EventEntries.GetEventActorsByUUIDAsync(customerId, uuid);
            var response = new ActorQueryResponse()
            {
                Actors = actors.Select(a => new Actor(a.UUID, a.Name, a.Email, a.IsForgotten)).Distinct().ToList()
            };
            return Ok(response);
        }

        [Authorize(Policy = "APIAccessOnly")]
        [HttpPost("{uuid}")]
        [ProducesResponseType(typeof(ActorQueryResponse), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ApiError), 400)]
        [ProducesResponseType(typeof(ApiError), 404)]
        public async Task<IActionResult> UpdateAsync(string uuid, [FromBody] ActorUpdateRequest actorUpdate)
        {
            var customerId = _membership.GetCustomerId(User);
            var actors = await _persistence.EventEntries.GetEventActorsByUUIDAsync(customerId, uuid);
            if (actors.Any(a => a.IsForgotten))
            {
                return NoContent();
            }
            else if (actors.Count == 0)
            {
                return NotFound(new ApiError(404, "Actor not found"));
            }

            var actor = await _persistence.EventEntries.UpdateEventActorAsync(customerId, uuid, actorUpdate.Name, actorUpdate.Email);
            var response = new ActorQueryResponse()
            {
                Actors = new List<Actor> {
                    new Actor(actor.UUID, actor.Name, actor.Email, actor.IsForgotten)
                }
            };
            return Ok(response);
        }

        [Authorize(Policy = "APIAccessOnly")]
        [HttpPost("{uuid}/forget")]
        [ProducesResponseType(typeof(ActorQueryResponse), 200)]
        [ProducesResponseType(typeof(ApiError), 400)]
        [ProducesResponseType(typeof(ApiError), 404)]
        public async Task<IActionResult> ForgetAsync(string uuid)
        {
            var customerId = _membership.GetCustomerId(User);
            var actors = await _persistence.EventEntries.GetEventActorsByUUIDAsync(customerId, uuid);
            if (actors.Count == 0)
            {
                return NotFound(new ApiError(404, "Actor not found"));
            }

            var actor = await _persistence.EventEntries.ForgetEventActorAsync(customerId, uuid);
            var response = new ActorQueryResponse()
            {
                Actors = new List<Actor> {
                    new Actor(actor.UUID, actor.Name, actor.Email, actor.IsForgotten)
                }
            };
            return Ok(response);
        }
    }
}
