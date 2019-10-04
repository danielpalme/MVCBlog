using System.Collections.Generic;

namespace MVCBlog.Web.Infrastructure.Mvc.SecurityHeaders
{
    public sealed class FeaturePolicyDirectiveBuilder
    {
        internal FeaturePolicyDirectiveBuilder()
        {
        }

        internal List<string> Sources { get; set; } = new List<string>();

        public FeaturePolicyDirectiveBuilder AllowSelf() => this.Allow("'self'");

        public FeaturePolicyDirectiveBuilder AllowNone() => this.Allow("'none'");

        public FeaturePolicyDirectiveBuilder AllowAny() => this.Allow("*");

        public FeaturePolicyDirectiveBuilder Allow(string source)
        {
            this.Sources.Add(source);
            return this;
        }
    }
}
