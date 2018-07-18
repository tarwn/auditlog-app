using AuditLogApp.Controllers.API.Public.Models.Events;
using AuditLogApp.Controllers.API.Public.Models.Events.Search;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditLogApp.Controllers.API.Public.Models.Views
{
    /// <summary>
    /// A paged set of events for a pre-configured View. The view is configured in the
    /// application to serve data to the drop-in component or your own custom
    /// component.
    /// </summary>
    public class PagedViewResult
    {
        public PagedViewResult(EventSearchId id)
        {
            Id = id;
            Links = new Dictionary<string, PagedViewLink>();
            Entries = new List<RecordedEvent>();
        }

        /// <summary>
        /// A generated "id" that represents this unique response
        /// </summary>
        [JsonProperty(PropertyName = "_id")]
        public EventSearchId Id { get; set; }

        /// <summary>
        /// Links to other pages of data you can navigate to from this set.
        /// </summary>
        [JsonProperty(PropertyName = "_links")]
        public Dictionary<string, PagedViewLink> Links { get; set; }

        /// <summary>
        /// The relevant Event Entries for this View
        /// </summary>
        public List<RecordedEvent> Entries { get; set; }
    }
}
