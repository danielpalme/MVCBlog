namespace MVCBlog.Business
{
    public class BlogSettings
    {
        public string BlogName { get; set; }

        public string BlogDescription { get; set; }

        public bool NotifyOnNewComments { get; set; } = true;

        public string NotifyOnNewCommentsEmail { get; set; }

        public string NotifyOnNewCommentsSubject { get; set; } = "New comment";

        public bool NewUsersCanRegister { get; set; }
    }
}
