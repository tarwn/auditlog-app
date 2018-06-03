using System.ComponentModel.DataAnnotations;

namespace AuditLogApp.Controllers.API.Public.Models.Events
{
    public class EventEntryContextServer
    {
        [Required]
        [StringLength(240)]
        public string ServerId { get; set; }

        [Required]
        [StringLength(80)]
        public string Version { get; set; }
    }
}