using AuditLogApp.Common.DTO;
using AuditLogApp.Common.Identity;
using AuditLogApp.Common.Persistence;
using AuditLogApp.Membership;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditLogApp.Controllers.API.Application
{
    [ApiVersion("1")]
    [Route("api/appln/v{version:apiVersion}/configuration")]
    [Authorize(Policy = "InteractiveAccessOnly")]
    public class CustomizationController : Controller
    {
        private IPersistenceStore _persistence;
        private IUserMembership _membership;

        public CustomizationController(IPersistenceStore persistence, IUserMembership membership)
        {
            _persistence = persistence;
            _membership = membership;
        }

        [HttpGet("views/default")]
        public async Task<IActionResult> GetOrCreateDefaultViewAsync()
        {
            var customerId = _membership.GetCustomerId(User);
            var view = await _persistence.Views.GetForCustomerAsync(customerId);
            if (view == null)
            {
                var accessKey = ConfigurationController.GenerateAPIKey();
                var newView = new ViewDTO(null, customerId, accessKey, new ViewCustomizationDTO("", "", "", new List<ViewCustomizationHeaderLinkDTO>(), ""), new List<ViewColumnDTO>());
                view = await _persistence.Views.CreateNewAsync(newView);
            }
            return Ok(new ViewConfigurationModel(view));
        }

        [HttpPost("views/default")]
        public async Task<IActionResult> UpdateViewAsync([FromBody] ViewConfigurationModel view)
        {
            var customerId = _membership.GetCustomerId(User);
            var viewDTO = new ViewDTO(
                view.Id,
                customerId,
                "----",
                new ViewCustomizationDTO(
                    view.Custom.Url,
                    view.Custom.Logo,
                    view.Custom.Title,
                    view.Custom.HeaderLinks.Select(hl => new ViewCustomizationHeaderLinkDTO(hl.Label, hl.Url))
                                           .ToList(),
                    view.Custom.Copyright),
                view.Columns.Select(c => new ViewColumnDTO(
                       c.Order,
                       c.Label,
                       c.Lines.Select(line => new ViewColumnLineDTO(line.Field))
                              .ToList()
                    )).ToList());
            viewDTO = await _persistence.Views.UpdateAsync(viewDTO);
            return Ok(new ViewConfigurationModel(viewDTO));
        }

        [HttpPost("views/default/resetKey")]
        public async Task<IActionResult> ResetKeyAsync([FromBody] Guid viewId)
        {
            var id = new ViewId(viewId);
            var customerId = _membership.GetCustomerId(User);
            var accessKey = ConfigurationController.GenerateAPIKey();
            var freshKey = await _persistence.Views.ResetKeyAsync(id, customerId, accessKey);
            return Ok(new
            {
                Id = id,
                Key = freshKey
            });
        }
    }
}
