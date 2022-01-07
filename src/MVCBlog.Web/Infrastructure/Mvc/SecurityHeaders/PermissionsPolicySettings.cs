namespace MVCBlog.Web.Infrastructure.Mvc.SecurityHeaders;

public class PermissionsPolicySettings
{
    public PermissionsPolicyDirectiveBuilder Accelerometer { get; } = new PermissionsPolicyDirectiveBuilder();

    public PermissionsPolicyDirectiveBuilder AmbientLightSensor { get; } = new PermissionsPolicyDirectiveBuilder();

    public PermissionsPolicyDirectiveBuilder Autoplay { get; } = new PermissionsPolicyDirectiveBuilder();

    public PermissionsPolicyDirectiveBuilder Camera { get; } = new PermissionsPolicyDirectiveBuilder();

    public PermissionsPolicyDirectiveBuilder DisplayCapture { get; } = new PermissionsPolicyDirectiveBuilder();

    public PermissionsPolicyDirectiveBuilder DocumentDomain { get; } = new PermissionsPolicyDirectiveBuilder();

    public PermissionsPolicyDirectiveBuilder EncryptedMedia { get; } = new PermissionsPolicyDirectiveBuilder();

    public PermissionsPolicyDirectiveBuilder ExecutionWhileNotRendered { get; } = new PermissionsPolicyDirectiveBuilder();

    public PermissionsPolicyDirectiveBuilder ExecutionWhileOutOfViewport { get; } = new PermissionsPolicyDirectiveBuilder();

    public PermissionsPolicyDirectiveBuilder Fullscreen { get; } = new PermissionsPolicyDirectiveBuilder();

    public PermissionsPolicyDirectiveBuilder Geolocation { get; } = new PermissionsPolicyDirectiveBuilder();

    public PermissionsPolicyDirectiveBuilder Gyroscope { get; } = new PermissionsPolicyDirectiveBuilder();

    public PermissionsPolicyDirectiveBuilder Magnetometer { get; } = new PermissionsPolicyDirectiveBuilder();

    public PermissionsPolicyDirectiveBuilder Microphone { get; } = new PermissionsPolicyDirectiveBuilder();

    public PermissionsPolicyDirectiveBuilder Midi { get; } = new PermissionsPolicyDirectiveBuilder();

    public PermissionsPolicyDirectiveBuilder Payment { get; } = new PermissionsPolicyDirectiveBuilder();

    public PermissionsPolicyDirectiveBuilder PictureInPicture { get; } = new PermissionsPolicyDirectiveBuilder();

    public PermissionsPolicyDirectiveBuilder PublickeyCredentials { get; } = new PermissionsPolicyDirectiveBuilder();

    public PermissionsPolicyDirectiveBuilder Speaker { get; } = new PermissionsPolicyDirectiveBuilder();

    public PermissionsPolicyDirectiveBuilder SyncXhr { get; } = new PermissionsPolicyDirectiveBuilder();

    public PermissionsPolicyDirectiveBuilder Usb { get; } = new PermissionsPolicyDirectiveBuilder();

    public PermissionsPolicyDirectiveBuilder WakeLock { get; } = new PermissionsPolicyDirectiveBuilder();

    public PermissionsPolicyDirectiveBuilder XR { get; } = new PermissionsPolicyDirectiveBuilder();
}
