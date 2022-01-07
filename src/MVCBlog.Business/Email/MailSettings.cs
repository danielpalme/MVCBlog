namespace MVCBlog.Business.Email;

public class MailSettings
{
    public bool SendMails { get; set; } = false;

    public string EmailHeaderTitle { get; set; } = null!;

    public string EmailHeaderUrl { get; set; } = null!;

    public string DefaultSenderName { get; set; } = null!;

    public string EmailAddress { get; set; } = null!;

    public string MailHost { get; set; } = null!;

    public int MailPort { get; set; } = 465;

    public string MailUserName { get; set; } = null!;

    public string MailPassword { get; set; } = null!;

    public bool UseSsl { get; set; } = true;

    public string? DefaultSignature { get; set; }
}