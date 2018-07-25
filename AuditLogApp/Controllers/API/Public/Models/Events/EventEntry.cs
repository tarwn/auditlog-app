using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuditLogApp.Controllers.API.Public.Models.Events
{
    /// <summary>
    /// Represents an Event that you are reporting from your system
    /// to be logged in the AuditLog. 
    /// 
    /// Events that you receive back from the API will include richer
    /// information, including a unique Id and Reception Time, but 
    /// otherwise match the same properties you sent.
    /// </summary>
    public class EventEntry
    {
        /// <summary>
        /// A unique identifier you generate for the event to ensure idempotent
        /// storage of the event.
        /// </summary>
        [Required]
        [StringLength(80)]
        public string UUID { get; set; }

        /// <summary>
        /// Information about the Client using your system, assuming users belong to
        /// more than one installation or customer.
        /// </summary>
        public EventEntryClient Client { get; set; }

        /// <summary>
        /// The time the Event occurred, provided in ISO8601 format.
        /// </summary>
        [Required]
        [DataType(DataType.DateTime, ErrorMessage = "Time must be provided in ISO8601 format")]
        public DateTime? Time { get; set; }

        /// <summary>
        /// The Action that occurred.
        /// </summary>
        [Required]
        [StringLength(40)]
        public string Action { get; set; }

        /// <summary>
        /// A human readable description of the action/event.
        /// </summary>
        [StringLength(120)]
        public string Description { get; set; }

        /// <summary>
        /// A URL to link to a change record or additional information for this Action/Event.
        /// </summary>
        [Url]
        [StringLength(400)]
        public string URL { get; set; }

        /// <summary>
        /// The Actor that made the change, either a user or artificial SYSTEM record.
        /// </summary>
        public EventEntryActor Actor { get; set; }

        /// <summary>
        /// Context from the Actor and your service for additional log detail.
        /// </summary>
        [Required]
        public EventEntryContext Context { get; set; }

        /// <summary>
        /// The Target that the Actor performed the Action against. Send a `TargetUser` if it is a User/Actor instead.
        /// </summary>
        public EventEntryTarget Target { get; set; }
        
        /// <summary>
        /// The Target User that the Actor performed the Action against. Send a `Target` if it is not a User/Actor instead.
        /// </summary>
        public EventEntryTargetUser TargetUser { get; set; }

    }
}
