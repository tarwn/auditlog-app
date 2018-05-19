using AuditLogApp.Common.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditLogApp.Membership.Data
{
    public class SessionDetails
    {
        public UserSessionId Id { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? LogoutTime { get; set; }

        public UserDetails User { get; set; }
    }
}
