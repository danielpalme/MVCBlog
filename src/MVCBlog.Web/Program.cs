using System.Globalization;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Net.Http.Headers;
using MVCBlog.Business;
using MVCBlog.Business.Email;
using MVCBlog.Business.IO;
using MVCBlog.Data;
using MVCBlog.Web.Infrastructure;
using MVCBlog.Web.Infrastructure.Mvc;
using MVCBlog.Web.Infrastructure.Mvc.Health;
using MVCBlog.Web.Infrastructure.Mvc.SecurityHeaders;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// Add builder.Services to the container.
builder.Services.AddDataProtection()
            .PersistKeysToFileSystem(new DirectoryInfo("DataEncrpytionKeys"));

builder.Services.Configure<BlogSettings>(builder.Configuration.GetSection("BlogSettings"));
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
});

builder.Services.AddDbContext<EFUnitOfWork>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("EFUnitOfWork")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<User>(config =>
{
    // Lockout settings
    config.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
    config.Lockout.MaxFailedAccessAttempts = 10;

    // User settings
    // TOOD: Enable if desired: config.SignIn.RequireConfirmedEmail = true;
    config.User.RequireUniqueEmail = true;
})
    .AddDefaultUI()
    .AddEntityFrameworkStores<EFUnitOfWork>()
    .AddErrorDescriber<LocalizedIdentityErrorDescriber>();

builder.Services.AddLocalization();

builder.Services.AddControllersWithViews()
    .AddViewLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddTransient<INotificationService, EmailNotificationService>();
builder.Services.AddTransient<IEmailSender, EmailSenderAdapter>();
builder.Services.AddTransient<IImageFileProvider, ImageFileProvider>();
builder.Services.AddTransient<IBlogEntryFileFileProvider, BlogEntryFileFileProvider>();

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
            builder.Services.AddScoped(interfaceType, serviceType);

            break;
        }
    }
}

builder.Services.Decorate(typeof(ICommandHandler<>), typeof(CommandLoggingDecorator<>));

builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration["ConnectionStrings:EFUnitOfWork"])
    .AddCheck<LogfileHealthCheck>("Log files");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        const int durationInSeconds = 24 * 60 * 60; // 24 hours
        ctx.Context.Response.Headers[HeaderNames.CacheControl] = "public,max-age=" + durationInSeconds;
    }
});
app.UseCookiePolicy();

app.UseSecurityHeaders(builder =>
{
    builder.PermissionsPolicySettings.Camera.AllowNone();

    builder.CspSettings.Defaults.AllowNone();
    builder.CspSettings.Connect.AllowSelf();
    builder.CspSettings.Manifest.AllowSelf();
    builder.CspSettings.Objects.AllowNone();
    builder.CspSettings.Frame.AllowNone();
    builder.CspSettings.Scripts.AllowSelf();

    builder.CspSettings.Styles
        .AllowSelf()
        .AllowUnsafeInline();

    builder.CspSettings.Fonts.AllowSelf();

    builder.CspSettings.Images
        .AllowSelf()
        .Allow("https://i2.wp.com")
        .Allow("https://www.gravatar.com");

    builder.CspSettings.BaseUri.AllowNone();
    builder.CspSettings.FormAction.AllowSelf();
    builder.CspSettings.FrameAncestors.AllowNone();

    builder.ReferrerPolicy = ReferrerPolicies.NoReferrerWhenDowngrade;
});

var supportedCultures = new[]
{
    new CultureInfo("en"),
    new CultureInfo("de")
};

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(supportedCultures[0]),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

app.UseStatusCodePagesWithReExecute("/Error/{0}");
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Blog}/{action=Index}/{id?}");
    endpoints.MapRazorPages();
});

app.UseHealthChecks("/health", new HealthCheckOptions()
{
    ResponseWriter = WriteResponse
});

Log.Logger = new LoggerConfiguration()
       .MinimumLevel.Information()
       .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
       .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Query", Serilog.Events.LogEventLevel.Error)
       .Enrich.FromLogContext()
       .WriteTo.RollingFile("Logs/Log-{Date}.txt", outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] [{SourceContext}] {Message}{NewLine}{Exception}")
       .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level >= Serilog.Events.LogEventLevel.Error).WriteTo.RollingFile("Logs/Errors/ErrorLog-{Date}.txt", outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] [{SourceContext}] {Message}{NewLine}{Exception}"))
       .CreateLogger();

using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var context = serviceScope.ServiceProvider.GetService<EFUnitOfWork>();
    context!.Database.Migrate();
}

try
{
    Log.Information("Starting application");
    app.Run();
    Log.Information("Stopped application");
    return 0;
}
catch (Exception exception)
{
    Log.Fatal(exception, "Application terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

static Task WriteResponse(HttpContext httpContext, HealthReport result)
{
    var writerOptions = new JsonWriterOptions
    {
        Indented = true
    };

    using (var ms = new MemoryStream())
    {
        using (var writer = new Utf8JsonWriter(ms, options: writerOptions))
        {
            writer.WriteStartObject();
            writer.WriteString("status", result.Status.ToString());
            writer.WriteStartObject("results");

            foreach (var kv in result.Entries)
            {
                writer.WriteStartObject(kv.Key);
                writer.WriteString("status", kv.Value.Status.ToString());
                writer.WriteString("description", kv.Value.Description);

                writer.WriteStartObject("data");
                foreach (var item in kv.Value.Data)
                {
                    writer.WriteString(item.Key, item.Value?.ToString());
                }

                writer.WriteEndObject();

                writer.WriteEndObject();
            }

            writer.WriteEndObject();
            writer.WriteEndObject();
        }

        httpContext.Response.ContentType = "application/json";
        return httpContext.Response.WriteAsync(Encoding.UTF8.GetString(ms.ToArray()));
    }
}