using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuditLogApp.Controllers.API.Public.Models.Events
{
    public class EventEntry
    {
        [Required]
        [StringLength(80)]
        public string UUID { get; set; }

        public EventEntryClient Client { get; set; }

        [Required]
        [DataType(DataType.DateTime, ErrorMessage = "Time must be provided in ISO8601 format")]
        public DateTime? Time { get; set; }

        [Required]
        [StringLength(40)]
        public string Action { get; set; }

        [StringLength(120)]
        public string Description { get; set; }

        [Url]
        [StringLength(400)]
        public string URL { get; set; }

        public EventEntryActor Actor { get; set; }

        [Required]
        public EventEntryContext Context { get; set; }

        public EventEntryTarget Target { get; set; }

    }
}
