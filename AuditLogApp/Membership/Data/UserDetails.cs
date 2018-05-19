using AuditLogApp.Common.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditLogApp.Membership.Data
{
    public class UserDetails
    {
        public UserId Id { get; set; }
        public string EmailAddress { get; set; }
        public string Username { get; set; }
    }
}
