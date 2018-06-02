namespace AuditLogApp.Controllers.API.Public.Models
{
    public class EventEntryContext
    {
        public EventEntryContextClient Client { get; set;  }

        public EventEntryContextServer Server { get; set; }
    }
}