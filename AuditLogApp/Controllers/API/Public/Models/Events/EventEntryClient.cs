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

        [Required]
        [StringLength(120)]
        public string UUID { get; set; }

        [StringLength(120)]
        public string Name { get; set; }
    }
}