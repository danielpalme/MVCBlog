using System.Collections.Generic;

namespace MVCBlog.Web.Infrastructure.Mvc.SecurityHeaders;

public sealed class PermissionsPolicyDirectiveBuilder
{
    internal PermissionsPolicyDirectiveBuilder()
    {
    }

    internal List<string> Sources { get; set; } = new List<string>();

    public PermissionsPolicyDirectiveBuilder AllowSelf() => this.Allow("self");

    public PermissionsPolicyDirectiveBuilder AllowNone() => this.Allow(string.Empty);

    public PermissionsPolicyDirectiveBuilder AllowAny() => this.Allow("*");

    public PermissionsPolicyDirectiveBuilder Allow(string source)
    {
        this.Sources.Add(source);
        return this;
    }
}
