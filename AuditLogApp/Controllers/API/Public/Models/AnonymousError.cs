using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditLogApp.Controllers.API.Public.Models
{
    public class AnonymousError
    {
        public string File { get; set; }
        public string Function { get; set; }
        public string DisplayedMessage { get; set; }
        public string Error { get; set; }
    }
}
