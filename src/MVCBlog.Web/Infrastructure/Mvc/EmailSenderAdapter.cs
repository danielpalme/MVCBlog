using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using MVCBlog.Business.Email;

namespace MVCBlog.Web.Infrastructure.Mvc;

public class EmailSenderAdapter : IEmailSender
{
    private readonly INotificationService notificationService;

    public EmailSenderAdapter(INotificationService notificationService)
    {
        this.notificationService = notificationService;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var message = new Message(new Recipient(email), subject, htmlMessage);
        await this.notificationService.SendNotificationAsync(message);
    }
}
