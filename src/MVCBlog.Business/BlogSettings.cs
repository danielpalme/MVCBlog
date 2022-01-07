namespace MVCBlog.Business;

public class BlogSettings
{
    public string BlogName { get; set; } = null!;

    public string BlogDescription { get; set; } = null!;

    public bool NotifyOnNewComments { get; set; } = true;

    public string? NotifyOnNewCommentsEmail { get; set; }

    public string NotifyOnNewCommentsSubject { get; set; } = "New comment";

    public bool NewUsersCanRegister { get; set; }
}