namespace MVCBlog.Web.Infrastructure.Mvc.SecurityHeaders;

public sealed class SecurityHeadersOptionsBuilder
{
    private readonly SecurityHeaderOptions options = new SecurityHeaderOptions();

    internal SecurityHeadersOptionsBuilder()
    {
    }

    public PermissionsPolicySettings PermissionsPolicySettings { get; } = new PermissionsPolicySettings();

    public CspSettings CspSettings { get; } = new CspSettings();

    public ReferrerPolicies ReferrerPolicy { get; set; } = ReferrerPolicies.None;

    internal SecurityHeaderOptions Build()
    {
        this.options.PermissionsPolicyHeader = this.GetPermissionsPolicyHeaderValue();
        this.options.CspHeader = this.GetCspHeaderValue();
        this.options.XFrameOptionsHeader = this.GetXFrameOptionsHeaderValue();
        this.options.ReferrerPolicyHeader = this.GetReferrerPolicyHeader();

        return this.options;
    }

    private string GetPermissionsPolicyHeaderValue()
    {
        var value = string.Empty;
        value += this.GetPermissionsDirective("accelerometer", this.PermissionsPolicySettings.Accelerometer.Sources);
        value += this.GetPermissionsDirective("ambient-light-sensor", this.PermissionsPolicySettings.AmbientLightSensor.Sources);
        value += this.GetPermissionsDirective("autoplay", this.PermissionsPolicySettings.Autoplay.Sources);
        value += this.GetPermissionsDirective("camera", this.PermissionsPolicySettings.Camera.Sources);
        value += this.GetPermissionsDirective("display-capture", this.PermissionsPolicySettings.DisplayCapture.Sources);
        value += this.GetPermissionsDirective("document-domain", this.PermissionsPolicySettings.DocumentDomain.Sources);
        value += this.GetPermissionsDirective("encrypted-media", this.PermissionsPolicySettings.EncryptedMedia.Sources);
        value += this.GetPermissionsDirective("execution-while-not-rendered", this.PermissionsPolicySettings.ExecutionWhileNotRendered.Sources);
        value += this.GetPermissionsDirective("execution-while-out-of-viewport", this.PermissionsPolicySettings.ExecutionWhileOutOfViewport.Sources);
        value += this.GetPermissionsDirective("fullscreen", this.PermissionsPolicySettings.Fullscreen.Sources);
        value += this.GetPermissionsDirective("geolocation", this.PermissionsPolicySettings.Geolocation.Sources);
        value += this.GetPermissionsDirective("gyroscope", this.PermissionsPolicySettings.Gyroscope.Sources);
        value += this.GetPermissionsDirective("magnetometer", this.PermissionsPolicySettings.Magnetometer.Sources);
        value += this.GetPermissionsDirective("microphone", this.PermissionsPolicySettings.Microphone.Sources);
        value += this.GetPermissionsDirective("midi", this.PermissionsPolicySettings.Midi.Sources);
        value += this.GetPermissionsDirective("payment", this.PermissionsPolicySettings.Payment.Sources);
        value += this.GetPermissionsDirective("picture-in-picture", this.PermissionsPolicySettings.PictureInPicture.Sources);
        value += this.GetPermissionsDirective("publickey-credentials", this.PermissionsPolicySettings.PublickeyCredentials.Sources);
        value += this.GetPermissionsDirective("speaker", this.PermissionsPolicySettings.Speaker.Sources);
        value += this.GetPermissionsDirective("sync-xhr", this.PermissionsPolicySettings.SyncXhr.Sources);
        value += this.GetPermissionsDirective("usb", this.PermissionsPolicySettings.Usb.Sources);
        value += this.GetPermissionsDirective("wake-lock", this.PermissionsPolicySettings.WakeLock.Sources);
        value += this.GetPermissionsDirective("xr", this.PermissionsPolicySettings.XR.Sources);

        if (value.EndsWith(", "))
        {
            value = value.Substring(0, value.Length - 2);
        }

        return value;
    }

    private string GetCspHeaderValue()
    {
        var value = string.Empty;
        value += this.GetDirective("default-src", this.CspSettings.Defaults.Sources);
        value += this.GetDirective("manifest-src", this.CspSettings.Manifest.Sources);
        value += this.GetDirective("connect-src", this.CspSettings.Connect.Sources);
        value += this.GetDirective("object-src", this.CspSettings.Objects.Sources);
        value += this.GetDirective("frame-src", this.CspSettings.Frame.Sources);
        value += this.GetDirective("script-src", this.CspSettings.Scripts.Sources);
        value += this.GetDirective("style-src", this.CspSettings.Styles.Sources);
        value += this.GetDirective("img-src", this.CspSettings.Images.Sources);
        value += this.GetDirective("font-src", this.CspSettings.Fonts.Sources);
        value += this.GetDirective("media-src", this.CspSettings.Media.Sources);
        value += this.GetDirective("base-uri", this.CspSettings.BaseUri.Sources);
        value += this.GetDirective("form-action", this.CspSettings.FormAction.Sources);
        value += this.GetDirective("frame-ancestors", this.CspSettings.FrameAncestors.Sources);
        return value;
    }

    private string GetPermissionsDirective(string directive, List<string> sources)
        => sources.Count > 0 ? $"{directive}=({string.Join(" ", sources)}), " : string.Empty;

    private string GetDirective(string directive, List<string> sources)
        => sources.Count > 0 ? $"{directive} {string.Join(" ", sources)}; " : string.Empty;

    private string GetXFrameOptionsHeaderValue()
    {
        // See https://infosec.mozilla.org/guidelines/web_security#x-frame-options
        if (this.CspSettings.FrameAncestors.Sources.Count == 1
            && this.CspSettings.FrameAncestors.Sources[0] == "'self'")
        {
            return "SAMEORIGIN";
        }
        else
        {
            return "DENY";
        }
    }

    private string? GetReferrerPolicyHeader()
    {
        switch (this.ReferrerPolicy)
        {
            case ReferrerPolicies.NoReferrer:
                return "no-referrer";
            case ReferrerPolicies.NoReferrerWhenDowngrade:
                return "no-referrer-when-downgrade";
            case ReferrerPolicies.SameOrigin:
                return "same-origin";
            case ReferrerPolicies.Origin:
                return "origin";
            case ReferrerPolicies.StrictOrigin:
                return "strict-origin";
            case ReferrerPolicies.OriginWhenCrossOrigin:
                return "origin-when-cross-origin";
            case ReferrerPolicies.StrictOriginWhenCrossOrigin:
                return "strict-origin-when-cross-origin";
            case ReferrerPolicies.UnsafeUrl:
                return "unsafe-url";
            case ReferrerPolicies.Empty:
                return string.Empty;
            case ReferrerPolicies.None:
            default:
                return null;
        }
    }
}
