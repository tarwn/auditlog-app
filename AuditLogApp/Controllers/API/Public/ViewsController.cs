using AuditLogApp.Common.Identity;
using AuditLogApp.Common.Persistence;
using AuditLogApp.Controllers.API.Public.Models.Events;
using AuditLogApp.Controllers.API.Public.Models.Events.Search;
using AuditLogApp.Controllers.API.Public.Models.Views;
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
    [Route("api/v{version:apiVersion}/views")]
    public class ViewsController : Controller
    {
        private IPersistenceStore _persistence;
        private ICustomerViewMembership _membership;

        public ViewsController(IPersistenceStore persistence, ICustomerViewMembership membership)
        {
            _persistence = persistence;
            _membership = membership;
        }

        [Authorize(Policy = "ViewAPIAccessOnly")]
        [HttpGet("{viewId:guid}/{clientUUId}")]
        [ProducesResponseType(typeof(PagedViewResult), 200)]
        [ProducesResponseType(typeof(ApiError), 400)]
        [ProducesResponseType(typeof(ApiError), 404)]
        public async Task<IActionResult> GetViewPageAsync(Guid viewId, string clientUUId, string page)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiError(400, ModelState));
            }

            var loginViewId = _membership.GetViewId(User);
            var urlViewId = new ViewId(viewId);
            if (loginViewId != urlViewId)
            {
                return BadRequest(new ApiError(400, "View Id mismatch"));
            }

            var customerId = _membership.GetCustomerId(User);
            // don't need to get view - we currently only query by client id paged by month, they are not configurable in View yet
            int year = DateTime.UtcNow.Year,
                month = DateTime.UtcNow.Month;

            if (!String.IsNullOrEmpty(page))
            {
                if (page.Length != 6)
                {
                    return BadRequest(new ApiError(400, "Unrecognizeable page requested (1)"));
                }

                if (!int.TryParse(page.Substring(0, 4), out year) ||
                    !int.TryParse(page.Substring(4, 2), out month))
                {
                    return BadRequest(new ApiError(400, "Unrecognizeable page requested (2)"));
                }
            }
            var pageStart = new DateTime(year, month, 1, 0, 0, 0, DateTimeKind.Utc);
            var pageEnd = pageStart.AddMonths(1);
            var entries = await _persistence.EventEntries.SearchAsync(customerId, clientUUId, pageStart, pageEnd);

            var prevPage = pageStart.AddMonths(-1);
            var nextPage = pageStart.AddMonths(1);

            var result = new PagedViewResult(new EventSearchId(DateTime.UtcNow, "GET", $"/api/v1/views/{viewId}/{clientUUId}?page={pageStart.ToString("yyyyMM")}", pageStart.ToString("MMMM yyyy")));
            if (prevPage > DateTime.UtcNow.AddMonths(-12) && entries.Count > 0)
            {
                result.Links.Add("previous", new PagedViewLink(prevPage.ToString("MMMM yyyy"), "GET", $"/api/v1/views/{viewId}/{clientUUId}?page={prevPage.ToString("yyyyMM")}"));
            }
            if (nextPage < DateTime.UtcNow)
            {
                result.Links.Add("next", new PagedViewLink(nextPage.ToString("MMMM yyyy"), "GET", $"/api/v1/views/{viewId}/{clientUUId}?page={nextPage.ToString("yyyyMM")}"));
            }
            result.Entries = entries.Select(e => new RecordedEvent(e, "/api/v1/events/"))
                                     .ToList();

            return Ok(result);
        }

    }
}
