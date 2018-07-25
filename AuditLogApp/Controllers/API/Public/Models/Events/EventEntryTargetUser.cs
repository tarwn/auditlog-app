using System.ComponentModel.DataAnnotations;

namespace AuditLogApp.Controllers.API.Public.Models.Events
{
    /// <summary>
    /// The User / Actor that has been altered / targeted
    /// </summary>
    public class EventEntryTargetUser
    {
        public EventEntryTargetUser() { }
        public EventEntryTargetUser(string uuid, string name, string email)
        {
            UUID = uuid;
            Name = name;
            Email = email;
        }

        /// <summary>
        /// A universally unique ID you can use to identify the actor against your own records.
        /// </summary>
        [Required]
        [StringLength(80)]
        public string UUID { get; set; }

        /// <summary>
        /// A human-readable name for the Actor for easy identification at a glance.
        /// </summary>
        [StringLength(120)]
        public string Name { get; set; }

        /// <summary>
        /// A human-readable email address for the Actor for easy identification at a glance.
        /// </summary>
        [StringLength(120)]
        public string Email { get; set; }
    }
}