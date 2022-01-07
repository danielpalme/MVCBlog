namespace MVCBlog.Web.Infrastructure.Mvc.SecurityHeaders;

public class CspSettings
{
    public CspDirectiveBuilder Defaults { get; } = new CspDirectiveBuilder();

    public CspDirectiveBuilder Manifest { get; } = new CspDirectiveBuilder();

    public CspDirectiveBuilder Connect { get; } = new CspDirectiveBuilder();

    public CspDirectiveBuilder Objects { get; } = new CspDirectiveBuilder();

    public CspDirectiveBuilder Frame { get; } = new CspDirectiveBuilder();

    public CspDirectiveBuilder Scripts { get; } = new CspDirectiveBuilder();

    public CspDirectiveBuilder Styles { get; } = new CspDirectiveBuilder();

    public CspDirectiveBuilder Images { get; } = new CspDirectiveBuilder();

    public CspDirectiveBuilder Fonts { get; } = new CspDirectiveBuilder();

    public CspDirectiveBuilder Media { get; } = new CspDirectiveBuilder();

    public CspDirectiveBuilder BaseUri { get; } = new CspDirectiveBuilder();

    public CspDirectiveBuilder FormAction { get; } = new CspDirectiveBuilder();

    public CspDirectiveBuilder FrameAncestors { get; } = new CspDirectiveBuilder();
}
