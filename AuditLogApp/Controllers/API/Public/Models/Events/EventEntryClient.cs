using System.ComponentModel.DataAnnotations;

namespace AuditLogApp.Controllers.API.Public.Models.Events
{
    public class EventEntryClient
    {
        [Required]
        [StringLength(120)]
        public string Id { get; set; }

        [StringLength(120)]
        public string Name { get; set; }
    }
}