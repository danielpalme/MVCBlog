using System.Collections.Generic;

namespace MVCBlog.Web.Infrastructure.Mvc.SecurityHeaders;

public sealed class CspDirectiveBuilder
{
    internal CspDirectiveBuilder()
    {
    }

    internal List<string> Sources { get; set; } = new List<string>();

    public CspDirectiveBuilder AllowSelf() => this.Allow("'self'");

    public CspDirectiveBuilder AllowUnsafeInline() => this.Allow("'unsafe-inline'");

    public CspDirectiveBuilder AllowNone() => this.Allow("'none'");

    public CspDirectiveBuilder AllowAny() => this.Allow("*");

    public CspDirectiveBuilder Allow(string source)
    {
        this.Sources.Add(source);
        return this;
    }
}
