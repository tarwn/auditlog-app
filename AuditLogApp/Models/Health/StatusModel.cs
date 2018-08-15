using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditLogApp.Models.Health
{
    public class StatusModel
    {
        public bool IsGood
        {
            get
            {
                return WebServer.Status == StatusEnum.Good &&
                    SQLServer.Status == StatusEnum.Good;
            }
        }

        public StatusModelDetail WebServer { get; set; }
        public StatusModelDetail SQLServer { get; set; }
    }
}
