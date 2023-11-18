namespace MVCBlog.Web.Infrastructure.Mvc.SecurityHeaders;

public sealed class SecurityHeadersMiddleware
{
    private const string PERMISSIONSPOLICYHEADER = "Permissions-Policy";

    private const string CSPHEADER = "Content-Security-Policy";

    private const string XFRAMEOPTIONSHEADER = "X-Frame-Options";

    private const string XXSSPROTECTIONHEADER = "X-XSS-Protection";

    private const string XCONTENTTYPEOPTIONSHEADER = "X-Content-Type-Options";

    private const string REFERRERPOLICYHEADER = "Referrer-Policy";

    private readonly RequestDelegate next;

    private readonly SecurityHeaderOptions options;

    public SecurityHeadersMiddleware(RequestDelegate next, SecurityHeaderOptions options)
    {
        this.next = next;
        this.options = options;
    }

    public async Task Invoke(HttpContext context)
    {
        if (!string.IsNullOrEmpty(this.options.PermissionsPolicyHeader))
        {
            context.Response.Headers.Append(PERMISSIONSPOLICYHEADER, this.options.PermissionsPolicyHeader);
        }

        if (!string.IsNullOrEmpty(this.options.CspHeader))
        {
            context.Response.Headers.Append(CSPHEADER, this.options.CspHeader);
        }

        context.Response.Headers.Append(XFRAMEOPTIONSHEADER, this.options.XFrameOptionsHeader);
        context.Response.Headers.Append(XXSSPROTECTIONHEADER, "1; mode=block");
        context.Response.Headers.Append(XCONTENTTYPEOPTIONSHEADER, "nosniff");

        if (this.options.ReferrerPolicyHeader != null)
        {
            context.Response.Headers.Append(REFERRERPOLICYHEADER, this.options.ReferrerPolicyHeader);
        }

        await this.next(context);
    }
}
