using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditLogApp.Controllers.API.Public.Models.Actors
{
    /// <summary>
    /// Represents the latest data for an Actor in the system, including GDPR "Forgotten" status.
    /// </summary>
    public class Actor
    {
        public Actor(string uuid, string name, string email, bool isForgotten)
        {
            this.UUID = uuid;
            this.Name = name;
            this.Email = email;
            this.IsForgotten = isForgotten;
        }

        /// <summary>
        /// A universally unique Id that identifies the user in your system.
        /// </summary>
        public string UUID { get; set; }

        /// <summary>
        /// A human-readable name for at a glance information.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A human-readable email address for at a glance information.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Indicator that the personally identifiable information on this record has been redacted.
        /// </summary>
        public bool IsForgotten { get; set; }
    }
}
