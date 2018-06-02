using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AuditLogApp.Documentation;
using AuditLogApp.Models.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditLogApp.Controllers
{
    [IncludeInDocumentation]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ValuesController : Controller
    {
        [Authorize(Policy = "APIAccessOnly")]
        [HttpGet("verify/api/ok")]
        [ProducesResponseType(typeof(VerifyModel), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(ApiError), 404)]
        public IActionResult GetApiOk()
        {
            return Ok(new VerifyModel()
            {
                SampleNumber = 42,
                SampleString = "Hello"
            });
        }

        [Authorize(Policy = "APIAccessOnly")]
        [HttpGet("verify/api/exception")]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(ApiError), 404)]
        public IActionResult GetApiException()
        {
            int[] x = null;
            return Ok(x.Length);
        }

        [Authorize(Policy = "APIAccessOnly")]
        [HttpPost("verify/api/400")]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        public IActionResult PostApiBadRequest(VerifyModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok();
        }

        [Authorize(Policy = "APIAccessOnly")]
        [HttpGet("verify/api/404")]
        [ProducesResponseType(typeof(ApiError), 404)]
        public IActionResult GetApiNotFound()
        {
            return NotFound(new ApiError(404, "Sample thing not found"));
        }
    }

    public class VerifyModel
    {
        public VerifyModel() { }

        [Required]
        [StringLength(maximumLength: 10, MinimumLength = 3)]
        public string SampleString { get; set; }

        [Required]
        public int SampleNumber { get; set; }

    }
}
