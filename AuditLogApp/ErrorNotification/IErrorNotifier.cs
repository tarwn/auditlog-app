using AuditLogApp.Models.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuditLogApp.ErrorNotification
{
    public interface IErrorNotifier
    {
        Task NotifyAsync(DescriptiveError descriptiveError, Exception exc, string path, ClaimsPrincipal user = null);
    }
}
