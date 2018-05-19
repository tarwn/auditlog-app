using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditLogApp.Membership.Data
{
    public class AuthenticationDetails
    {
        public string Id { get; set; }
        public string CredentialType { get; set; }
        public string Identity { get; set; }
        public string DisplayName { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
