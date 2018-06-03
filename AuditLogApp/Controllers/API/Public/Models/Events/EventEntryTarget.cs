using System.ComponentModel.DataAnnotations;

namespace AuditLogApp.Controllers.API.Public.Models.Events
{
    public class EventEntryTarget
    {
        public EventEntryTarget() { }
        public EventEntryTarget(string type, string uuid, string label, string url)
        {
            Type = type;
            UUID = uuid;
            Label = label;
            URL = url;
        }

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