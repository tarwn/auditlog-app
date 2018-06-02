using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditLogApp.Membership
{
    public class CustomerAPIAuthOptions : AuthenticationSchemeOptions
    {
        public string AuthenticationScheme { get; set; }
        public string WWWAuthenticateRealm { get; set; }
        public string HTTPHeader { get; set; }
    }
}
