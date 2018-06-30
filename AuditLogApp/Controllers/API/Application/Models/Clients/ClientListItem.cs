using AuditLogApp.Common.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditLogApp.Controllers.API.Application.Models.Clients
{
    public class ClientListItem
    {
        public ClientListItem(EventClientId id, string uuid, string name)
        {
            this.Id = id;
            this.UUID = uuid;
            this.Name = name;
        }

        public EventClientId Id { get; set; }
        public string UUID { get; set; }
        public string Name { get; set; }
    }
}
