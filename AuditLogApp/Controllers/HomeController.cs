using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuditLogApp.Common.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace AuditLogApp.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private IPersistenceStore _persistence;

        public HomeController(IPersistenceStore persistence)
        {
            _persistence = persistence;
        }

        [HttpGet()]
        public async Task<IActionResult> IndexAsync()
        {
            var customer = await _persistence.Customers.GetAsync(1);

            return View("Index", customer);
        }
    }
}