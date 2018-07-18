using System.ComponentModel.DataAnnotations;

namespace AuditLogApp.Controllers.API.Public.Models.Events
{
    public class EventEntryTarget
    {
        public EventEntryTarget() { }
        public EventEntryTarget(string type, string uuid, string label, string url)
        {
            Type = type;
            UUID = uuid;
            Label = label;
            URL = url;
        }

        /// <summary>
        /// The Type of the Target being affected. There is only one special type 'User', all others can be defined custom by you.
        /// </summary>
        [Required]
        [StringLength(40)]
        public string Type { get; set; }

        /// <summary>
        /// A universally unique Id for the Target being affected that you can connect back to the record on your systems.
        /// </summary>
        [Required]
        [StringLength(80)]
        public string UUID { get; set; }

        /// <summary>
        /// A human-readable description of the target being affected, for at a glance information. Do not use for 'User'.
        /// </summary>
        [StringLength(120)]
        public string Label { get; set; }

        /// <summary>
        /// A URL reference back to the target being affected, if useful.
        /// </summary>
        [StringLength(400)]
        [Url]
        public string URL { get; set; }
    }
}