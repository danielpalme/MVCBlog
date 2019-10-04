namespace MVCBlog.Web.Infrastructure.Mvc.SecurityHeaders
{
    public class FeaturePolicySettings
    {
        public FeaturePolicyDirectiveBuilder Accelerometer { get; } = new FeaturePolicyDirectiveBuilder();

        public FeaturePolicyDirectiveBuilder AmbientLightSensor { get; } = new FeaturePolicyDirectiveBuilder();

        public FeaturePolicyDirectiveBuilder Autoplay { get; } = new FeaturePolicyDirectiveBuilder();

        public FeaturePolicyDirectiveBuilder Camera { get; } = new FeaturePolicyDirectiveBuilder();

        public FeaturePolicyDirectiveBuilder DisplayCapture { get; } = new FeaturePolicyDirectiveBuilder();

        public FeaturePolicyDirectiveBuilder DocumentDomain { get; } = new FeaturePolicyDirectiveBuilder();

        public FeaturePolicyDirectiveBuilder EncryptedMedia { get; } = new FeaturePolicyDirectiveBuilder();

        public FeaturePolicyDirectiveBuilder ExecutionWhileNotRendered { get; } = new FeaturePolicyDirectiveBuilder();

        public FeaturePolicyDirectiveBuilder ExecutionWhileOutOfViewport { get; } = new FeaturePolicyDirectiveBuilder();

        public FeaturePolicyDirectiveBuilder Fullscreen { get; } = new FeaturePolicyDirectiveBuilder();

        public FeaturePolicyDirectiveBuilder Geolocation { get; } = new FeaturePolicyDirectiveBuilder();

        public FeaturePolicyDirectiveBuilder Gyroscope { get; } = new FeaturePolicyDirectiveBuilder();

        public FeaturePolicyDirectiveBuilder Magnetometer { get; } = new FeaturePolicyDirectiveBuilder();

        public FeaturePolicyDirectiveBuilder Microphone { get; } = new FeaturePolicyDirectiveBuilder();

        public FeaturePolicyDirectiveBuilder Midi { get; } = new FeaturePolicyDirectiveBuilder();

        public FeaturePolicyDirectiveBuilder Payment { get; } = new FeaturePolicyDirectiveBuilder();

        public FeaturePolicyDirectiveBuilder PictureInPicture { get; } = new FeaturePolicyDirectiveBuilder();

        public FeaturePolicyDirectiveBuilder PublickeyCredentials { get; } = new FeaturePolicyDirectiveBuilder();

        public FeaturePolicyDirectiveBuilder Speaker { get; } = new FeaturePolicyDirectiveBuilder();

        public FeaturePolicyDirectiveBuilder SyncXhr { get; } = new FeaturePolicyDirectiveBuilder();

        public FeaturePolicyDirectiveBuilder Usb { get; } = new FeaturePolicyDirectiveBuilder();

        public FeaturePolicyDirectiveBuilder WakeLock { get; } = new FeaturePolicyDirectiveBuilder();

        public FeaturePolicyDirectiveBuilder XR { get; } = new FeaturePolicyDirectiveBuilder();
    }
}