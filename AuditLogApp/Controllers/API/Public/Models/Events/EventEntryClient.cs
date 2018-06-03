using System.ComponentModel.DataAnnotations;

namespace AuditLogApp.Controllers.API.Public.Models.Events
{
    public class EventEntryClient
    {

        public EventEntryClient() { }
        public EventEntryClient(string id, string name)
        {
            Id = id;
            Name = name;
        }

        [Required]
        [StringLength(120)]
        public string Id { get; set; }

        [StringLength(120)]
        public string Name { get; set; }
    }
}