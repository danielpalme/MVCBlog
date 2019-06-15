using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MVCBlog.Business;
using MVCBlog.Business.Email;
using MVCBlog.Business.IO;
using MVCBlog.Data;
using MVCBlog.Web.Infrastructure;
using MVCBlog.Web.Infrastructure.Mvc;
using MVCBlog.Web.Infrastructure.Mvc.Health;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MVCBlog.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo("DataEncrpytionKeys"));

            services.Configure<BlogSettings>(this.Configuration.GetSection("BlogSettings"));
            services.Configure<MailSettings>(this.Configuration.GetSection("MailSettings"));

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<EFUnitOfWork>(options =>
                options.UseSqlServer(
                    this.Configuration.GetConnectionString("EFUnitOfWork")));

            services.AddDefaultIdentity<User>(config =>
                {
                    // Lockout settings
                    config.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                    config.Lockout.MaxFailedAccessAttempts = 10;

                    // User settings
                    // TOOD: Enable if desired: config.SignIn.RequireConfirmedEmail = true;
                    config.User.RequireUniqueEmail = true;
                })
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<EFUnitOfWork>()
                .AddErrorDescriber<Infrastructure.LocalizedIdentityErrorDescriber>();

            services.AddLocalization();

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddViewLocalization(options => options.ResourcesPath = "Resources");

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<INotificationService, EmailNotificationService>();
            services.AddTransient<IEmailSender, EmailSenderAdapter>();
            services.AddTransient<IImageFileProvider, ImageFileProvider>();
            services.AddTransient<IBlogEntryFileFileProvider, BlogEntryFileFileProvider>();

            // Add CommandHandlers with decoractor
            // See https://github.com/khellang/Scrutor
            foreach (var serviceType in typeof(ICommandHandler<>).Assembly.GetTypes())
            {
                if (serviceType.IsAbstract || serviceType.IsInterface || serviceType.BaseType == null)
                {
                    continue;
                }

                foreach (var interfaceType in serviceType.GetInterfaces())
                {
                    if (interfaceType.IsGenericType && typeof(ICommandHandler<>).IsAssignableFrom(interfaceType.GetGenericTypeDefinition()))
                    {
                        // Register service
                        services.AddScoped(interfaceType, serviceType);

                        break;
                    }
                }
            }

            services.Decorate(typeof(ICommandHandler<>), typeof(CommandLoggingDecorator<>));

            services.AddHealthChecks()
                .AddSqlServer(this.Configuration["ConnectionStrings:EFUnitOfWork"])
                .AddCheck<LogfileHealthCheck>("Log files");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            var supportedCultures = new[]
            {
                new CultureInfo("en"),
                new CultureInfo("de"),
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(supportedCultures[0]),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseStatusCodePagesWithReExecute("/Error/{0}");

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Blog}/{action=Index}/{id?}");
            });

            app.UseHealthChecks("/health", new HealthCheckOptions()
            {
                ResponseWriter = WriteResponse
            });

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<EFUnitOfWork>();
                context.Database.Migrate();
            }
        }

        private static Task WriteResponse(HttpContext httpContext, HealthReport result)
        {
            httpContext.Response.ContentType = "application/json";

            var json = new JObject(
                new JProperty("status", result.Status.ToString()),
                new JProperty("results", new JObject(result.Entries.Select(pair =>
                    new JProperty(pair.Key, new JObject(
                        new JProperty("status", pair.Value.Status.ToString()),
                        new JProperty("description", pair.Value.Description),
                        new JProperty("data", new JObject(pair.Value.Data.Select(
                            p => new JProperty(p.Key, p.Value))))))))));
            return httpContext.Response.WriteAsync(
                json.ToString(Formatting.Indented));
        }
    }
}
