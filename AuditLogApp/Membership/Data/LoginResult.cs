using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditLogApp.Membership.Data
{
    public class LoginResult
    {
        public bool Failed { get; set; }

        public static LoginResult GetFailed()
        {
            return new LoginResult()
            {
                Failed = true
            };
        }

        public static LoginResult GetSuccess()
        {
            return new LoginResult()
            {
                Failed = false
            };
        }
    }
}
