using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AuditLogApp.Common.Persistence;
using AuditLogApp.Membership;
using AuditLogApp.Membership.Implementation;
using AuditLogApp.Persistence.SQLServer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AuditLogApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.RespectBrowserAcceptHeader = true;
                // Add XML Content Negotiation
                options.InputFormatters.Add(new XmlSerializerInputFormatter());
                options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
                // Don't default to JSON when there isn't a good answer
                options.ReturnHttpNotAcceptable = true;
            });

            services.AddSingleton<IPersistenceStore>((s) =>
            {
                return new PersistenceStore(Configuration["SQL:ConnectionString"]);
            });

            // add error handling here
            
            // Authentication
            services.AddAuditLogInteractiveAuthentication<PersistedUserMembership>((options) =>
            {
                options.InteractiveAuthenticationType = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultPathAfterLogin = "/";
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                /* API Authentication Provider */
                //.AddAuditLogAPIAuthentication("AL-API-Token", "AuditLog.co")
                /* 3rd Party Auth Providers */
                .AddCookie("ExternalProvidersCookie")
                .AddTwitter("Twitter", options =>
                {
                    options.SignInScheme = "ExternalProvidersCookie";

                    options.ConsumerKey = Configuration["Authentication:Twitter:ConsumerKey"];
                    options.ConsumerSecret = Configuration["Authentication:Twitter:ConsumerSecret"];
                })
                /* Interactive 'Session' Cookie Provider */
                .AddCookie((options) =>
                {
                    options.LoginPath = new PathString("/account/login");
                    options.LogoutPath = new PathString("/account/logout");
                    options.Events = new CookieAuthenticationEvents()
                    {
                        OnValidatePrincipal = async (c) =>
                        {
                            var membership = c.HttpContext.RequestServices.GetRequiredService<IUserMembership>();
                            var isValid = await membership.ValidateLoginAsync(c.Principal);
                            if (!isValid)
                            {
                                c.RejectPrincipal();
                            }
                        }
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("APIAccessOnly", policy =>
                {
                    policy.AddAuthenticationSchemes("AL-API-Token");
                    policy.RequireAuthenticatedUser();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Content/build"))
            });

            app.UseMvc();
        }
    }
}
