using System.Linq;
using MVCBlog.Business.Email;
using Xunit;

namespace MVCBlog.Business.Test.Email;

public class MessageTest
{
    [Fact]
    public void Constructor()
    {
        var message = new Message(new Recipient("test@test.de"), "Test", "<p>Hello</p>");

        Assert.Equal(1, message.BccRecipients.Count);
        Assert.Equal("test@test.de", message.BccRecipients.First().Address);
        Assert.Equal("Test", message.Subject);
        Assert.Equal("<p>Hello</p>", message.BodyAsHtml);

        message = new Message(new[] { new Recipient("test@test.de"), new Recipient("test2@test.de") }, "Test", "<p>Hello</p>");

        Assert.Equal(2, message.BccRecipients.Count);
        Assert.Equal("test@test.de", message.BccRecipients.First().Address);
        Assert.Equal("test2@test.de", message.BccRecipients.ElementAt(1).Address);
        Assert.Equal("Test", message.Subject);
        Assert.Equal("<p>Hello</p>", message.BodyAsHtml);
    }

    [Fact]
    public void WithSender()
    {
        var message = new Message(new Recipient("test@test.de"), "Test", "<p>Hello</p>")
            .WithSender(new Recipient("sender@test.de"));

        Assert.Equal("sender@test.de", message.Sender!.Address);
    }

    [Fact]
    public void WithReplyTo()
    {
        var message = new Message(new Recipient("test@test.de"), "Test", "<p>Hello</p>")
            .WithReplyTo(new Recipient("replyto@test.de"));

        Assert.Equal("replyto@test.de", message.ReplyTo!.Address);
    }

    [Fact]
    public void AddRecipient()
    {
        var message = new Message(new Recipient("test@test.de"), "Test", "<p>Hello</p>")
            .AddRecipient(new Recipient("test2@test.de"));

        Assert.Equal(2, message.BccRecipients.Count);
        Assert.Equal("test@test.de", message.BccRecipients.First().Address);
        Assert.Equal("test2@test.de", message.BccRecipients.ElementAt(1).Address);
    }

    [Fact]
    public void AddAttachment()
    {
        var message = new Message(new Recipient("test@test.de"), "Test", "<p>Hello</p>")
            .AddAttachment(new Attachment("test.jpg", new byte[0]));

        Assert.Equal(1, message.Attachments.Count);
    }

    [Fact]
    public void AddEmbeddedImage()
    {
        var message = new Message(new Recipient("test@test.de"), "Test", "<p>Hello</p>")
            .AddEmbeddedImage(new EmbeddedImage("test.jpg", new byte[0], "test.jpg"));

        Assert.Equal(1, message.EmbeddedImages.Count);
    }
}
