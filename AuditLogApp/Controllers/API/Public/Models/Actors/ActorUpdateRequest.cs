using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditLogApp.Controllers.API.Public.Models.Actors
{
    public class ActorUpdateRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
