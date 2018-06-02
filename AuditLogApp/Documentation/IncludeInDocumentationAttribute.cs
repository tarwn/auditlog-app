using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditLogApp.Documentation
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class IncludeInDocumentationAttribute : Attribute
    {
    }
}
