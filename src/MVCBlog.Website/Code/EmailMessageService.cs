using System;
using System.Net.Mail;
using System.Web;
using MVCBlog.Core.Service;

namespace MVCBlog.Website
{
    public class EmailMessageService : IMessageService
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(EmailMessageService));

        /// <summary>
        /// Sends an email.
        /// </summary>
        /// <param name="sender">Email address of the sender.</param>
        /// <param name="recipient">Email address of the recipient.</param>
        /// <param name="replyto">Email address to reply to (optional).</param>
        /// <param name="subject">Subject of the email.</param>
        /// <param name="body">Body of the email.</param>
        /// <returns><c>true</c> if email was sent; otherwise <c>false</c>.</returns>
        public bool SendMessage(string sender, string recipient, string replyto, string subject, string body)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }

            if (recipient == null)
            {
                throw new ArgumentNullException("recipient");
            }

            if (subject == null)
            {
                throw new ArgumentNullException("subject");
            }

            if (body == null)
            {
                throw new ArgumentNullException("body");
            }

            var request = HttpContext.Current.Request;
            body = "IP: " + request.UserHostAddress + "\nReferrer: " + request.UrlReferrer + "\n\n" + body;

            var message = new MailMessage(sender, recipient);

            if (!string.IsNullOrEmpty(replyto))
            {
                message.ReplyToList.Add(new MailAddress(replyto));
            }

            message.From = new MailAddress(sender);
            message.Subject = subject;
            message.Body = body;

            try
            {
                var client = new SmtpClient();
                client.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("Sending email failed", ex);
                return false;
            }
        }
    }
}