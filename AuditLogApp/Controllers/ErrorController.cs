using AuditLogApp.ErrorNotification;
using AuditLogApp.Models.Error;
using AuditLogApp.Models.Shared;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditLogApp.Controllers
{

    [Route("Error")]
    public class ErrorController : Controller
    {
        private IErrorNotifier _notifier;

        public ErrorController(IErrorNotifier notifier)
        {
            _notifier = notifier;
        }

        public async Task<IActionResult> ErrorAsync()
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionFeature == null)
            {
                return View("GenericError");
            }

            var descriptiveError = GetDescriptiveError(exceptionFeature.Error);

            await _notifier.NotifyAsync(descriptiveError, exceptionFeature.Error, exceptionFeature.Path, User);

            if (IsApiCall(exceptionFeature.Path))
            {
                return new ObjectResult(descriptiveError)
                {
                    StatusCode = 500
                };
            }

            return View("DescriptiveError", descriptiveError);
        }

        [HttpGet("{statusCode}")]
        public IActionResult Error(int statusCode)
        {
            var reexecuteFeature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            if (reexecuteFeature != null && IsApiCall(reexecuteFeature.OriginalPath))
            {
                return new ObjectResult(new ApiError(statusCode));
            }

            if (statusCode == 404)
            {
                return View("404");
            }

            return View("GenericError");
        }

        private bool IsApiCall(string path)
        {
            return path.StartsWith("/api/") || path.StartsWith("/verify/api/");
        }

        private DescriptiveError GetDescriptiveError(Exception error)
        {
            var descriptiveError = new DescriptiveError("An internal error has occurred");

            // add details if available

            return descriptiveError;
        }
    }
}
