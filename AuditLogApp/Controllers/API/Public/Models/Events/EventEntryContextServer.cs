using System.ComponentModel.DataAnnotations;

namespace AuditLogApp.Controllers.API.Public.Models.Events
{
    public class EventEntryContextServer
    {
        public EventEntryContextServer() { }
        public EventEntryContextServer(string serverId, string version)
        {
            ServerId = serverId;
            Version = version;
        }

        [Required]
        [StringLength(240)]
        public string ServerId { get; set; }

        [Required]
        [StringLength(80)]
        public string Version { get; set; }
    }
}