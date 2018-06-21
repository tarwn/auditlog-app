using AuditLogApp.Common.Enums;
using AuditLogApp.Common.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuditLogApp.Membership
{
    public interface ICustomerViewMembership
    {
        Task<ClaimsPrincipal> GetAuditViewAsync(ViewId viewId, string accessKey);
        CustomerId GetCustomerId(ClaimsPrincipal user);
        ViewId GetViewId(ClaimsPrincipal user);
    }
}
