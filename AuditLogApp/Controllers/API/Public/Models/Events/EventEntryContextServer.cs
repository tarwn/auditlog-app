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

        /// <summary>
        /// A server Id for the server the Actor is connected to, if applicable.
        /// </summary>
        [Required]
        [StringLength(240)]
        public string ServerId { get; set; }

        /// <summary>
        /// The version of your software the Actor is connected to, if applicable.
        /// </summary>
        [Required]
        [StringLength(80)]
        public string Version { get; set; }
    }
}