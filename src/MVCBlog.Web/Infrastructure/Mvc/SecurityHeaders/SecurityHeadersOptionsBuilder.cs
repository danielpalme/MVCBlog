using System.Collections.Generic;

namespace MVCBlog.Web.Infrastructure.Mvc.SecurityHeaders
{
    public sealed class SecurityHeadersOptionsBuilder
    {
        private readonly SecurityHeaderOptions options = new SecurityHeaderOptions();

        internal SecurityHeadersOptionsBuilder()
        {
        }

        public FeaturePolicySettings FeaturePolicySettings { get; } = new FeaturePolicySettings();

        public CspSettings CspSettings { get; } = new CspSettings();

        public ReferrerPolicies ReferrerPolicy { get; set; } = ReferrerPolicies.None;

        internal SecurityHeaderOptions Build()
        {
            this.options.FeaturePolicyHeader = this.GetFeaturePolicyHeaderValue();
            this.options.CspHeader = this.GetCspHeaderValue();
            this.options.XFrameOptionsHeader = this.GetXFrameOptionsHeaderValue();
            this.options.ReferrerPolicyHeader = this.GetReferrerPolicyHeader();

            return this.options;
        }

        private string GetFeaturePolicyHeaderValue()
        {
            var value = string.Empty;
            value += this.GetDirective("accelerometer", this.FeaturePolicySettings.Accelerometer.Sources);
            value += this.GetDirective("ambient-light-sensor", this.FeaturePolicySettings.AmbientLightSensor.Sources);
            value += this.GetDirective("autoplay", this.FeaturePolicySettings.Autoplay.Sources);
            value += this.GetDirective("camera", this.FeaturePolicySettings.Camera.Sources);
            value += this.GetDirective("display-capture", this.FeaturePolicySettings.DisplayCapture.Sources);
            value += this.GetDirective("document-domain", this.FeaturePolicySettings.DocumentDomain.Sources);
            value += this.GetDirective("encrypted-media", this.FeaturePolicySettings.EncryptedMedia.Sources);
            value += this.GetDirective("execution-while-not-rendered", this.FeaturePolicySettings.ExecutionWhileNotRendered.Sources);
            value += this.GetDirective("execution-while-out-of-viewport", this.FeaturePolicySettings.ExecutionWhileOutOfViewport.Sources);
            value += this.GetDirective("fullscreen", this.FeaturePolicySettings.Fullscreen.Sources);
            value += this.GetDirective("geolocation", this.FeaturePolicySettings.Geolocation.Sources);
            value += this.GetDirective("gyroscope", this.FeaturePolicySettings.Gyroscope.Sources);
            value += this.GetDirective("magnetometer", this.FeaturePolicySettings.Magnetometer.Sources);
            value += this.GetDirective("microphone", this.FeaturePolicySettings.Microphone.Sources);
            value += this.GetDirective("midi", this.FeaturePolicySettings.Midi.Sources);
            value += this.GetDirective("payment", this.FeaturePolicySettings.Payment.Sources);
            value += this.GetDirective("picture-in-picture", this.FeaturePolicySettings.PictureInPicture.Sources);
            value += this.GetDirective("publickey-credentials", this.FeaturePolicySettings.PublickeyCredentials.Sources);
            value += this.GetDirective("speaker", this.FeaturePolicySettings.Speaker.Sources);
            value += this.GetDirective("sync-xhr", this.FeaturePolicySettings.SyncXhr.Sources);
            value += this.GetDirective("usb", this.FeaturePolicySettings.Usb.Sources);
            value += this.GetDirective("wake-lock", this.FeaturePolicySettings.WakeLock.Sources);
            value += this.GetDirective("xr", this.FeaturePolicySettings.XR.Sources);
            return value;
        }

        private string GetCspHeaderValue()
        {
            var value = string.Empty;
            value += this.GetDirective("default-src", this.CspSettings.Defaults.Sources);
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

        private string GetReferrerPolicyHeader()
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
}