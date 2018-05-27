using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuditLogApp.Common.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditLogApp.Controllers
{
    [Route("")]
    [Authorize(Policy = "InteractiveAccessOnly")]
    public class ApplicationController : Controller
    {
        private IPersistenceStore _persistence;

        public ApplicationController(IPersistenceStore persistence)
        {
            _persistence = persistence;
        }

        [HttpGet()]
        public IActionResult Index()
        {
            return View("Index");
        }
    }
}