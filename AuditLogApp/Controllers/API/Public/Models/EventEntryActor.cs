using System.ComponentModel.DataAnnotations;

namespace AuditLogApp.Controllers.API.Public.Models
{
    public class EventEntryActor
    {
        [Required]
        [StringLength(80)]
        public string UUID { get; set; }

        [StringLength(120)]
        public string Name { get; set; }

        [StringLength(120)]
        public string Email { get; set; }
    }
}