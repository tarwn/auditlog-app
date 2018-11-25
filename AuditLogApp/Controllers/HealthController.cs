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
                WebServer = GetWebServerStatus(),
                SQLServer = await GetSQLServerStatusAsync()
            };

            return StatusCode(status.IsGood ? 200 : 500, status);
        }

        private StatusModelDetail GetWebServerStatus()
        {
            return new StatusModelDetail(StatusEnum.Good, "Ok");
        }

        private async Task<StatusModelDetail> GetSQLServerStatusAsync()
        {
            try
            {
                var sqlServerCheck = await _persistence.Server.GetLatestVersionAsync();
                if (sqlServerCheck != null)
                {
                    return new StatusModelDetail(StatusEnum.Good, "Ok");
                }
                else
                {
                    return new StatusModelDetail(StatusEnum.Degraded, "No server version available");
                }
            }
            catch (Exception exc)
            {
                return new StatusModelDetail(StatusEnum.Unreachable, $"SQL Server Error: [{exc.GetType().Name}] {exc.Message}");
            }
        }

    }
}
