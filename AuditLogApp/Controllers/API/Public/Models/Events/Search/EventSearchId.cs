using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace AuditLogApp.Controllers.API.Public.Models.Events.Search
{
    /// <summary>
    /// Generated id to represent a query for Event Entries
    /// </summary>
    public class EventSearchId
    {
        public EventSearchId(DateTime timestamp, string httpMethod, string href, string label)
        {
            Timestamp = timestamp;
            Type = httpMethod;
            Href = href;
            Label = label;
        }

        /// <summary>
        /// Timestamp that the results were put together
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// The type of request
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The Href for this specific response
        /// </summary>
        public string Href { get; set; }

        /// <summary>
        /// A human-readable label for an end user looking at this dataset
        /// </summary>
        public string Label { get; set; }
    }
}
