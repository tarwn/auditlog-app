using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuditLogApp.Common.DTO;
using AuditLogApp.Common.Identity;
using Newtonsoft.Json;

namespace AuditLogApp.Controllers.API.Public.Models.Events
{
    public class RecordedEvent
    {
        public RecordedEvent(EventEntryDTO entry, string baseUrl)
        {
            Link = new RecordedEventLink() { Href = $"{baseUrl}{entry.Id.RawValue}" };
            Id = entry.Id;
            ReceptionTime = entry.ReceptionTime;

            UUID = entry.UUID;
            Client = new EventEntryClient(entry.Client_UUID, entry.Client_Name);
            Time = entry.EventTime;
            Action = entry.Action;
            Description = entry.Description;
            URL = entry.URL;
            Actor = new EventEntryActor(entry.Actor_UUID, entry.Actor_Name, entry.Actor_Email);
            Context = new EventEntryContext(entry.Context_Client_IP, entry.Context_Client_BrowserAgent, entry.Context_Server_ServerId, entry.Context_Server_Version);
            Target = new EventEntryTarget(entry.Target_Type, entry.Target_UUID, entry.Target_Label, entry.Target_URL);
        }

        [JsonProperty(PropertyName = "_links")]
        public RecordedEventLink Link { get; set; }

        public EventEntryId Id { get; set; }
        public DateTime ReceptionTime { get; set; }


        public string UUID { get; set; }

        public EventEntryClient Client { get; set; }

        public DateTime Time { get; set; }

        public string Action { get; set; }

        public string Description { get; set; }

        public string URL { get; set; }

        public EventEntryActor Actor { get; set; }

        public EventEntryContext Context { get; set; }

        public EventEntryTarget Target { get; set; }
    }

    public class RecordedEventLink
    {
        [JsonProperty(PropertyName = "href")]
        public string Href { get; set; }
    }
}

