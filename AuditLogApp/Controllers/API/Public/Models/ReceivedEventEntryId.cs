using AuditLogApp.Common.Identity;
using System;

namespace AuditLogApp.Controllers.API.Public.Models
{
    public class ReceivedEventEntryId
    {
        public ReceivedEventEntryId() { }
        public ReceivedEventEntryId(EventEntryId id, string uuid) {
            Id = id.RawValue;
            UUID = uuid;
        }

        public Guid Id { get; set; }
        public string UUID { get; set; }
    }
}