using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditLogApp.Membership
{
    public static class CustomerViewAPIAuthHandlerExtensions
    {        
        public static AuthenticationBuilder AddCustomerViewAPIAuth(this AuthenticationBuilder builder, string scheme, Action<CustomerViewAPIAuthOptions> options)
        {
            return builder.AddScheme<CustomerViewAPIAuthOptions, CustomerViewAPIAuthHandler>(scheme, o =>
            {
                o.AuthenticationScheme = scheme;
                options(o);
            });
        }
    }
}
