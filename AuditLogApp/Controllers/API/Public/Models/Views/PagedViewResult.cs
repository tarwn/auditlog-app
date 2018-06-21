using AuditLogApp.Controllers.API.Public.Models.Events;
using AuditLogApp.Controllers.API.Public.Models.Events.Search;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditLogApp.Controllers.API.Public.Models.Views
{
    public class PagedViewResult
    {
        public PagedViewResult(EventSearchId id)
        {
            Id = id;
            Links = new Dictionary<string, PagedViewLink>();
            Entries = new List<RecordedEvent>();
        }

        [JsonProperty(PropertyName = "_id")]
        public EventSearchId Id { get; set; }

        [JsonProperty(PropertyName = "_links")]
        public Dictionary<string, PagedViewLink> Links { get; set; }

        public List<RecordedEvent> Entries { get; set; }
    }
}
