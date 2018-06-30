using AuditLogApp.Controllers.API.Public.Models.Events;
using AuditLogApp.Controllers.API.Public.Models.Events.Search;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditLogApp.Controllers.API.Application.Models.Events
{
    public class ApplicationEventSearchResponse
    {
        [JsonProperty(PropertyName = "_id")]
        public EventSearchId Id { get; set; }

        public List<RecordedEvent> Entries { get; set; }
    }
}
