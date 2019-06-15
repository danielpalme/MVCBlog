namespace MVCBlog.Business.Email
{
    public class MailSettings
    {
        public bool SendMails { get; set; } = false;

        public string EmailHeaderTitle { get; set; }

        public string EmailHeaderUrl { get; set; }

        public string DefaultSenderName { get; set; }

        public string EmailAddress { get; set; }

        public string MailHost { get; set; }

        public int MailPort { get; set; } = 465;

        public string MailUserName { get; set; }

        public string MailPassword { get; set; }

        public bool UseSsl { get; set; } = true;

        public string DefaultSignature { get; set; }
    }
}