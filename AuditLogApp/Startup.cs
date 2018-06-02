using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using AuditLogApp.Common.Identity;
using AuditLogApp.Common.Persistence;
using AuditLogApp.Documentation;
using AuditLogApp.ErrorNotification;
using AuditLogApp.Membership;
using AuditLogApp.Membership.Implementation;
using AuditLogApp.Models.Error;
using AuditLogApp.Persistence.SQLServer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

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
            // things that used to work correctly
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            // Dependencies
            RegisterDependencies(services);

            // MVC
            services.AddMvcCore().AddVersionedApiExplorer(o => {
                o.GroupNameFormat = "'v'VVV";
                o.SubstituteApiVersionInUrl = true;
            }); 
            services.AddMvc(options =>
            {
                options.RespectBrowserAcceptHeader = true;
                // Add XML Content Negotiation
                options.InputFormatters.Add(new XmlSerializerInputFormatter());
                options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
                // Don't default to JSON when there isn't a good answer
                options.ReturnHttpNotAcceptable = true;

            })
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.Converters.Add(new IdentityJsonConverter<Int32>());
                options.SerializerSettings.Converters.Add(new IdentityJsonConverter<Guid>());
            });

            // API Versioning

            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
            });

            // Documentation

            services.AddSwaggerGen(o =>
            {
                var provider = services.BuildServiceProvider()
                                   .GetRequiredService<IApiVersionDescriptionProvider>();

                o.DocInclusionPredicate((docName, apiDesc) =>
                {
                    // must be opted into documentation and match version/group name
                    return apiDesc.ControllerAttributes()
                        .OfType<IncludeInDocumentationAttribute>()
                        .Any()
                        &&
                        apiDesc.ControllerAttributes()
                            .OfType<ApiVersionAttribute>()
                            .SelectMany(v => v.Versions)
                            .Any(v => $"v{v.ToString()}" == docName);
                });

                o.AddSecurityDefinition("APIKey", new ApiKeyScheme() {
                    Type = "apiKey",
                    Name = "X-API-KEY",
                    In = "header"
                });
                o.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> {
                    { "APIKey", new[] { "APIAccessOnly" } }
                });
                o.OperationFilter<SecurityRequirementsOperationFilter>();

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    o.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
                }
            });

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

                    options.CallbackPath = new PathString("/account/3rdparty/twitter/sign-in");
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
                            try
                            {
                                var membership = c.HttpContext.RequestServices.GetRequiredService<IUserMembership>();
                                var isValid = await membership.ValidateLoginAsync(c.Principal);
                                if (!isValid)
                                {
                                    c.RejectPrincipal();
                                }
                            }
                            catch (Exception exc)
                            {
                                var errorService = c.HttpContext.RequestServices.GetRequiredService<IErrorNotifier>();
                                await errorService.NotifyAsync(new DescriptiveError("Error during validation of session"), exc, c.Request.Path);
                                c.RejectPrincipal();
                            }
                        }
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("InteractiveAccessOnly", policy =>
                {
                    policy.AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                });

                options.AddPolicy("APIAccessOnly", policy =>
                {
                    policy.AddAuthenticationSchemes("AL-API-Token");
                    policy.RequireAuthenticatedUser();
                });
            });
        }

        private Info CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new Info()
            {
                Title = $"AuditLog.co API {description.ApiVersion}",
                Version = description.ApiVersion.ToString(),
                Description = "API for AuditLog.co",
                Contact = new Contact() { Name = "Eli Weinstock-Herman", Email = "eli@auditlog.co" },
                //TermsOfService = "Shareware",
                License = new License() { Name = "Commercial" }
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }

        private void RegisterDependencies(IServiceCollection services)
        {
            // Persistence 

            services.AddScoped<IPersistenceStore>((s) =>
            {
                return new PersistenceStore(Configuration["SQL:ConnectionString"]);
            });

            // Error Handling

            services.AddScoped<SmtpClient>((s) =>
            {
                if (Configuration["smtp:DeliveryMethod"] == "directory")
                {
                    return new SmtpClient()
                    {
                        DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                        PickupDirectoryLocation = Path.Combine(Environment.ContentRootPath, "..", "mail")
                    };
                }
                else
                {
                    return new SmtpClient(Configuration["smtp:Host"], int.Parse(Configuration["smtp:Port"]))
                    {
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        EnableSsl = bool.Parse(Configuration["smtp:EnableSsl"]),
                        Credentials = new NetworkCredential(Configuration["smtp:Username"], Configuration["smtp:Password"])
                    };
                }
            });

            services.AddEmailErrorNotifier((options) =>
            {
                options.EnvironmentName = Environment.EnvironmentName;
                options.FromAddress = Configuration["EmailAddresses:ErrorsFrom"];
                options.ToAddress = Configuration["EmailAddresses:ErrorsTo"];
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApiVersionDescriptionProvider provider)
        {
            // Error Handling
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            app.UseExceptionHandler("/error");
            app.UseStatusCodePagesWithReExecute("/error/{0}");

            // Authentication
            app.UseAuthentication();

            // Rewrite SPA routes to index
            var options = new RewriteOptions();
            if (!env.IsDevelopment())
            {
                options.AddRedirectToHttps();
            }
            options.AddRewrite("^configure/.*", "/", skipRemainingRules: true)
                   .AddRewrite("^welcome(/.*)?", "/onboarding", skipRemainingRules: true);
            app.UseRewriter(options);

            // Static assets
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Content/build"))
            });

            // MVC route handling
            app.UseMvc();
            app.UseSwagger();
            //app.UseSwaggerUI(o =>
            //{
            //    o.SwaggerEndpoint($"/swagger/v1/swagger.json", "v1");
            //});
        }
    }
}
