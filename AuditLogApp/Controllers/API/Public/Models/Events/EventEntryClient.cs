using System.ComponentModel.DataAnnotations;

namespace AuditLogApp.Controllers.API.Public.Models.Events
{
    public class EventEntryClient
    {

        public EventEntryClient() { }
        public EventEntryClient(string uuid, string name)
        {
            UUID = uuid;
            Name = name;
        }

        /// <summary>
        /// A Unique Identifier for this client that you can map back to your internal systems.
        /// </summary>
        [Required]
        [StringLength(120)]
        public string UUID { get; set; }

        /// <summary>
        /// A human-readable description of the client for easy identification at a glance.
        /// </summary>
        [StringLength(120)]
        public string Name { get; set; }
    }
}