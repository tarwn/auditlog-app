using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditLogApp.Controllers.API.Public.Models.Actors
{
    public class Actor
    {
        public Actor(string uuid, string name, string email, bool isForgotten)
        {
            this.UUID = uuid;
            this.Name = name;
            this.Email = email;
            this.IsForgotten = isForgotten;
        }

        public string UUID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsForgotten { get; set; }
    }
}
