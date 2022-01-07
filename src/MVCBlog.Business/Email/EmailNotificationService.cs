using Microsoft.Extensions.Options;
using MimeKit;

namespace MVCBlog.Business.Email;

public class EmailNotificationService : INotificationService
{
    private readonly MailSettings mailSettings;

    public EmailNotificationService(IOptions<MailSettings> optionsAccessor)
    {
        this.mailSettings = optionsAccessor.Value;
    }

    public async Task SendNotificationAsync(Message message)
    {
        if (message == null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        var mimeMessage = new MimeMessage()
        {
            Subject = message.Subject
        };

        if (message.Sender != null)
        {
            mimeMessage.From.Add(Convert(message.Sender));
        }
        else
        {
            mimeMessage.From.Add(new MailboxAddress(this.mailSettings.DefaultSenderName, this.mailSettings.EmailAddress));
        }

        if (message.ReplyTo != null)
        {
            mimeMessage.ReplyTo.Add(Convert(message.ReplyTo));
        }

        foreach (var recipient in message.BccRecipients)
        {
            mimeMessage.Bcc.Add(Convert(recipient));
        }

        var builder = new BodyBuilder();

        foreach (var attachment in message.Attachments)
        {
            builder.Attachments.Add(attachment.FileName, attachment.Data);
        }

        foreach (var embeddedImage in message.EmbeddedImages)
        {
            var mimeEntity = builder.LinkedResources.Add(embeddedImage.FileName, embeddedImage.Data);
            mimeEntity.ContentId = embeddedImage.ContentId;
        }

        builder.HtmlBody = string.Format(
            Notifications.MailTemplate,
            this.mailSettings.EmailHeaderUrl,
            this.mailSettings.EmailHeaderTitle,
            message.Subject,
            message.BodyAsHtml,
            this.mailSettings.DefaultSignature?.Replace("\n", "<br />"));

        mimeMessage.Body = builder.ToMessageBody();

        if (this.mailSettings.SendMails)
        {
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                // TODO: Validate certificate?!

                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                await client.ConnectAsync(this.mailSettings.MailHost, this.mailSettings.MailPort, this.mailSettings.UseSsl);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                await client.AuthenticateAsync(this.mailSettings.MailUserName, this.mailSettings.MailPassword);

                await client.SendAsync(mimeMessage);
                await client.DisconnectAsync(true);
            }
        }
        else
        {
            string path = Path.Combine(Path.GetTempPath(), "maildrop");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            await mimeMessage.WriteToAsync(Path.Combine(path, "message" + DateTime.Now.ToString("yyyy.MM.dd_HH_mm_ss") + "_" + Guid.NewGuid().ToString("N") + ".eml"));
        }
    }

    private static MailboxAddress Convert(Recipient recipient)
    {
        var mailboxAdress = recipient.Name != null ?
                new MailboxAddress(recipient.Name, recipient.Address)
                : MailboxAddress.Parse(recipient.Address);

        return mailboxAdress;
    }
}