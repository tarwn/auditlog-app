using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditLogApp.Models.Error
{
    public class DescriptiveError
    {
        public DescriptiveError(string message)
        {
            Message = message;
            AdditionalDetails = new Dictionary<string, string>();
        }

        public string Message { get; set; }
        public Dictionary<string, string> AdditionalDetails { get; set; }
    }
}
