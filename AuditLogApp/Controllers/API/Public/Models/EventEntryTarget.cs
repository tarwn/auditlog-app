using System.ComponentModel.DataAnnotations;

namespace AuditLogApp.Controllers.API.Public.Models
{
    public class EventEntryTarget
    {
        [Required]
        [StringLength(40)]
        public string Type { get; set; }

        [Required]
        [StringLength(80)]
        public string UUID { get; set; }

        [StringLength(120)]
        public string Label { get; set; }
        
        [StringLength(400)]
        [Url]
        public string URL { get; set; }
    }
}