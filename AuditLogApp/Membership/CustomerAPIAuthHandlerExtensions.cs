using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditLogApp.Membership
{
    public static class CustomerAPIAuthHandlerExtensions
    {        
        public static AuthenticationBuilder AddCustomerAPIAuth(this AuthenticationBuilder builder, string scheme, Action<CustomerAPIAuthOptions> options)
        {
            return builder.AddScheme<CustomerAPIAuthOptions, CustomerAPIAuthHandler>(scheme, o =>
            {
                o.AuthenticationScheme = scheme;
                options(o);
            });
        }
    }
}
