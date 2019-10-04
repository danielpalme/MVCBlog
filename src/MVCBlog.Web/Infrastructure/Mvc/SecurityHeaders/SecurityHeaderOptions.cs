namespace MVCBlog.Web.Infrastructure.Mvc.SecurityHeaders
{
    public sealed class SecurityHeaderOptions
    {
        public string FeaturePolicyHeader { get; set; }

        public string CspHeader { get; set; }

        public string XFrameOptionsHeader { get; set; }

        public string ReferrerPolicyHeader { get; set; }
    }
}
