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

        public EventEntryContextClient Client { get; set;  }

        [Required]
        public EventEntryContextServer Server { get; set; }
    }
}