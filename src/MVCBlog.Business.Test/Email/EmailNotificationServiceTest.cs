using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MVCBlog.Business.Email;
using Xunit;

namespace MVCBlog.Business.Test.Email;

public class EmailNotificationServiceTest
{
    private readonly string pickupDirectoryLocation = Path.Combine(Path.GetTempPath(), "maildrop");

    private readonly IOptions<MailSettings> mailSettings = Options.Create(new MailSettings()
    {
        SendMails = false,
        EmailHeaderTitle = "My Blog",
        EmailHeaderUrl = "https://www.todo.com",
        DefaultSenderName = "TODO",
        EmailAddress = "todo@todo.com"
    });

    [Fact]
    public async Task SendNotification_EmailIsSendSucessfully()
    {
        if (Directory.Exists(this.pickupDirectoryLocation))
        {
            foreach (var file in Directory.GetFiles(this.pickupDirectoryLocation))
            {
                File.Delete(file);
            }
        }
        else
        {
            Directory.CreateDirectory(this.pickupDirectoryLocation);
        }

        var sut = new EmailNotificationService(this.mailSettings);

        await sut.SendNotificationAsync(new Message(new Recipient("info@test.de"), "Subject123", "Body123"));

        var files = Directory.GetFiles(this.pickupDirectoryLocation);

        Assert.True(Directory.Exists(this.pickupDirectoryLocation));
        Assert.Single(files);

        string message = File.ReadAllText(files[0]);

        Assert.Contains("info@test.de", message);
        Assert.Contains("Subject123", message);
    }

    [Fact]
    public void SendNotification_MessageNull_ArgumentNullException()
    {
        var sut = new EmailNotificationService(this.mailSettings);

        Assert.ThrowsAsync<ArgumentNullException>(() => sut.SendNotificationAsync(null!));
    }
}
