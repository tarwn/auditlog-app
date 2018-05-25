using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditLogApp.ErrorNotification
{
    public class EmailErrorNotifierOptions
    {
        public object EnvironmentName { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
    }
}
