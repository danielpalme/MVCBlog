using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MVCBlog.Web.Infrastructure.Mvc.SecurityHeaders
{
    public sealed class SecurityHeadersMiddleware
    {
        private const string FEATUREPOLICYHEADER = "Feature-Policy";

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
            if (!string.IsNullOrEmpty(this.options.FeaturePolicyHeader))
            {
                context.Response.Headers.Add(FEATUREPOLICYHEADER, this.options.FeaturePolicyHeader);
            }

            if (!string.IsNullOrEmpty(this.options.CspHeader))
            {
                context.Response.Headers.Add(CSPHEADER, this.options.CspHeader);
            }

            context.Response.Headers.Add(XFRAMEOPTIONSHEADER, this.options.XFrameOptionsHeader);
            context.Response.Headers.Add(XXSSPROTECTIONHEADER, "1; mode=block");
            context.Response.Headers.Add(XCONTENTTYPEOPTIONSHEADER, "nosniff");

            if (this.options.ReferrerPolicyHeader != null)
            {
                context.Response.Headers.Add(REFERRERPOLICYHEADER, this.options.ReferrerPolicyHeader);
            }

            await this.next(context);
        }
    }
}