using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditLogApp.Membership
{
    public static class UserMembershipExtensions
    {
        public static IServiceCollection AddAuditLogInteractiveAuthentication<T>(this IServiceCollection services, Action<UserMembershipOptions> options)
            where T : class, IUserMembership
        {
            services.AddTransient<IUserMembership, T>();
            services.AddTransient<UserMembershipOptions>((s) =>
            {
                var opts = new UserMembershipOptions();
                options(opts);
                return opts;
            });

            return services;
        }
    }
}
