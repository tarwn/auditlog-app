using AuditLogApp.Common.Identity;
using AuditLogApp.Common.Persistence;
using AuditLogApp.Controllers.API.Public.Models;
using AuditLogApp.Controllers.API.Public.Models.Configuration;
using AuditLogApp.Controllers.API.Public.Models.Events;
using AuditLogApp.Controllers.API.Public.Models.Events.Search;
using AuditLogApp.Controllers.API.Public.Models.Views;
using AuditLogApp.Documentation;
using AuditLogApp.ErrorNotification;
using AuditLogApp.Membership;
using AuditLogApp.Models.Error;
using AuditLogApp.Models.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditLogApp.Controllers.API.Public
{
    
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/errors")]
    public class AnonymousErrorController : Controller
    {
        private IErrorNotifier _notifier;
        private ICustomerViewMembership _membership;

        public AnonymousErrorController(IErrorNotifier notifier, ICustomerViewMembership membership)
        {
            _notifier = notifier;
            _membership = membership;
        }

        /// <summary>
        /// Reports an error that occurred in the drop-in
        /// </summary>
        [EnableCors("AllowDropInAccess")]
        [HttpPost("")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> ReportErrorAsync([FromBody] AnonymousError error)
        {
            var descriptiveError = GetDescriptiveError(error);
            descriptiveError.AdditionalDetails.Add("Source", "Drop-in");
            await _notifier.NotifyAsync(descriptiveError, true, User);
            return NoContent();
        }

        private DescriptiveError GetDescriptiveError(AnonymousError error)
        {
            var err = new DescriptiveError(error.DisplayedMessage);
            err.AdditionalDetails.Add("File", error.File);
            err.AdditionalDetails.Add("Function", error.Function);
            err.AdditionalDetails.Add("Error", error.Error);
            return err;
        }
    }
}
