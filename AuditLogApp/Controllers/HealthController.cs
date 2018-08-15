using AuditLogApp.Common.Persistence;
using AuditLogApp.Models.Health;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditLogApp.Controllers
{
    [ApiVersion("1")]
    [Route("health")]
    public class HealthController : Controller
    {
        private IPersistenceStore _persistence;

        public HealthController(IPersistenceStore persistence)
        {
            _persistence = persistence;
        }

        [HttpGet("status")]
        [Produces("application/json")]
        public async Task<IActionResult> GetStatusAsync()
        {
            var status = new StatusModel()
            {
                WebServer = new StatusModelDetail(StatusEnum.Good, "Ok")
            };

            try
            {
                var sqlServerCheck = await _persistence.Server.GetLatestVersionAsync();
                if (sqlServerCheck != null)
                {
                    status.SQLServer = new StatusModelDetail(StatusEnum.Good, "Ok");
                }
                else
                {
                    status.SQLServer = new StatusModelDetail(StatusEnum.Degraded, "No server version available");
                }
            }
            catch(Exception exc)
            {
                status.SQLServer = new StatusModelDetail(StatusEnum.Unreachable, $"SQL Server Error: [{exc.GetType().Name}] {exc.Message}");
            }


            if (status.IsGood)
            {
                return Ok(status);
            }
            else
            {
                return StatusCode(500, status);
            }
        }


    }
}
