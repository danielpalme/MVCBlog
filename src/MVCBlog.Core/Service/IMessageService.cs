namespace MVCBlog.Core.Service
{
    public interface IMessageService
    {
        /// <summary>
        /// Sends an email.
        /// </summary>
        /// <param name="sender">Email address of the sender.</param>
        /// <param name="recipient">Email address of the recipient.</param>
        /// <param name="replyto">Email address to reply to (optional).</param>
        /// <param name="subject">Subject of the email.</param>
        /// <param name="body">Body of the email.</param>
        /// <returns><c>true</c> if email was sent; otherwise <c>false</c>.</returns>
        bool SendMessage(string sender, string recipient, string replyto, string subject, string body);
    }
}
