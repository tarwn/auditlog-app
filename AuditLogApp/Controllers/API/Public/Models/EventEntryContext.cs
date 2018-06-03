using System.ComponentModel.DataAnnotations;

namespace AuditLogApp.Controllers.API.Public.Models
{
    public class EventEntryContext
    {
        public EventEntryContextClient Client { get; set;  }

        [Required]
        public EventEntryContextServer Server { get; set; }
    }
}