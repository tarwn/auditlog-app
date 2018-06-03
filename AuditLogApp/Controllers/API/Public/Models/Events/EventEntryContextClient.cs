using System.ComponentModel.DataAnnotations;

namespace AuditLogApp.Controllers.API.Public.Models.Events
{
    public class EventEntryContextClient
    {
        public EventEntryContextClient() { }
        public EventEntryContextClient(string ipAddress, string browserAgent)
        {
            IPAddress = ipAddress;
            BrowserAgent = browserAgent;
        }

        [StringLength(15)]
        [RegularExpression(@"[012]?\d?\d\.[012]?\d?\d\.[012]?\d?\d\.[012]?\d?\d", ErrorMessage = "This must be formatted as an IPv4 Address between 0.0.0.0 and 255.255.255.255")]
        public string IPAddress { get; set; }

        [StringLength(4096)]
        public string BrowserAgent { get; set; }
    }
}