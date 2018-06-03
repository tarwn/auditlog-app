using System.ComponentModel.DataAnnotations;

namespace AuditLogApp.Controllers.API.Public.Models.Events
{
    public class EventEntryActor
    {
        public EventEntryActor() { }
        public EventEntryActor(string uuid, string name, string email)
        {
            UUID = uuid;
            Name = name;
            Email = email;
        }

        [Required]
        [StringLength(80)]
        public string UUID { get; set; }

        [StringLength(120)]
        public string Name { get; set; }

        [StringLength(120)]
        public string Email { get; set; }
    }
}