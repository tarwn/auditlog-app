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
    [Route("api/appln/v{version:apiVersion}/customization")]
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
                view = await CreateDefaultViewAsync(customerId);
            }
            return Ok(new ViewConfigurationModel(view));
        }
        
        [HttpGet("views/dashboard")]
        public async Task<IActionResult> GetOrCreateDashboardViewAsync()
        {
            var customerId = _membership.GetCustomerId(User);
            var view = await _persistence.Views.GetForCustomerAsync(customerId);
            if (view == null)
            {
                view = await CreateDefaultViewAsync(customerId);
                // convert the default client view into a dashboard view, until we offer customizable dashboard views
                view.Columns.Insert(0, new ViewColumnDTO(0, "Client", new List<ViewColumnLineDTO>() {
                    new ViewColumnLineDTO("client.name"),
                    new ViewColumnLineDTO("client.uuid")
                }));
                for (int i = 0; i < view.Columns.Count; i++)
                {
                    view.Columns[i].Order = 0;
                }
            }
            return Ok(new ViewConfigurationModel(view));
        }

        [HttpPost("views/default")]
        public async Task<IActionResult> UpdateViewAsync([FromBody] ViewConfigurationModel view)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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


        private async Task<ViewDTO> CreateDefaultViewAsync(CustomerId customerId)
        {
            var customer = await _persistence.Customers.GetAsync(customerId);
            var accessKey = ConfigurationController.GenerateAPIKey();
            var newView = new ViewDTO(null, customerId, accessKey, new ViewCustomizationDTO("", "/images/logo56.png", customer.DisplayName, new List<ViewCustomizationHeaderLinkDTO>(), ""), CreateDefaultColumnLayout());
            return await _persistence.Views.CreateNewAsync(newView);
        }

        private List<ViewColumnDTO> CreateDefaultColumnLayout()
        {
            return new List<ViewColumnDTO>() {
                new ViewColumnDTO(0, "Action", new List<ViewColumnLineDTO>(){
                    new ViewColumnLineDTO("action"),
                    new ViewColumnLineDTO("description")
                }),
                new ViewColumnDTO(0, "Actor", new List<ViewColumnLineDTO>(){
                    new ViewColumnLineDTO("actor.name"),
                    new ViewColumnLineDTO("actor.uuid")
                }),
                new ViewColumnDTO(0, "Actor IP", new List<ViewColumnLineDTO>(){
                    new ViewColumnLineDTO("context.client.ipAddress")
                }),
                new ViewColumnDTO(0, "Target", new List<ViewColumnLineDTO>(){
                    new ViewColumnLineDTO("target.type"),
                    new ViewColumnLineDTO("target.label")
                }),
                new ViewColumnDTO(0, "Time", new List<ViewColumnLineDTO>(){
                    new ViewColumnLineDTO("time[time]")
                })
            };
        }
    }
}
