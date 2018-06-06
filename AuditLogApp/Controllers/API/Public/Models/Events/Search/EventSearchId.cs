using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace AuditLogApp.Controllers.API.Public.Models.Events.Search
{
    public class EventSearchId
    {
        public EventSearchId(DateTime timestamp, string httpMethod, string href, string label)
        {
            Timestamp = timestamp;
            Type = httpMethod;
            Href = href;
            Label = label;
        }

        public DateTime Timestamp { get; set; }
        public string Type { get; set; }
        public string Href { get; set; }
        public string Label { get; set; }
    }
}
