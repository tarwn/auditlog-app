using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditLogApp.Controllers.API.Public.Models
{
    public class EventAcceptedResponse
    {
        public EventAcceptedResponse(){
            ReceivedEvents = new List<ReceivedEventEntryId>();
        }

        public List<ReceivedEventEntryId> ReceivedEvents { get; set; }
    }
}
