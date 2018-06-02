using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditLogApp.Membership
{
    public static class MembershipExtensions
    {
        public static IServiceCollection AddAuditLogInteractiveAuthentication<T>(this IServiceCollection services, Action<MembershipOptions> options)
            where T : class, IUserMembership
        {
            services.AddTransient<IUserMembership, T>();
            services.AddTransient<MembershipOptions>((s) =>
            {
                var opts = new MembershipOptions();
                options(opts);
                return opts;
            });

            return services;
        }
    }
}
