using System.ComponentModel.DataAnnotations;

namespace AuditLogApp.Controllers.API.Public.Models.Events
{
    public class EventEntryContext
    {
        public EventEntryContext() { }
        public EventEntryContext(string clientIP, string clientBrowserAgent, string serverId, string serverVersion)
        {
            Client = new EventEntryContextClient(clientIP, clientBrowserAgent);
            Server = new EventEntryContextServer(serverId, serverVersion);
        }

        /// <summary>
        /// Properties of the Actor's client/system for audit purposes.
        /// </summary>
        public EventEntryContextClient Client { get; set;  }

        /// <summary>
        /// Properties of your software/server that they are interacting with.
        /// </summary>
        [Required]
        public EventEntryContextServer Server { get; set; }
    }
}