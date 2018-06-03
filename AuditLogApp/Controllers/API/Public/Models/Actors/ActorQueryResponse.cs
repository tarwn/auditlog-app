using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditLogApp.Controllers.API.Public.Models.Actors
{
    public class ActorQueryResponse
    {
        public List<Actor> Actors { get; set; }
    }
}
