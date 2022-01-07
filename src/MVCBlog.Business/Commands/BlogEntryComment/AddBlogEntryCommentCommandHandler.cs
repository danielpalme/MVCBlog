using System.Text;
using System.Web;
using Microsoft.Extensions.Options;
using MVCBlog.Business.Email;
using MVCBlog.Data;

namespace MVCBlog.Business.Commands;

public class AddBlogEntryCommentCommandHandler : ICommandHandler<AddBlogEntryCommentCommand>
{
    private readonly EFUnitOfWork unitOfWork;

    private readonly INotificationService notificationService;

    private readonly BlogSettings blogSettings;

    public AddBlogEntryCommentCommandHandler(
        EFUnitOfWork unitOfWork,
        INotificationService notificationService,
        IOptions<BlogSettings> optionsAccessor)
    {
        this.unitOfWork = unitOfWork;
        this.notificationService = notificationService;
        this.blogSettings = optionsAccessor.Value;
    }

    public async Task HandleAsync(AddBlogEntryCommentCommand command)
    {
        this.unitOfWork.BlogEntryComments.Add(command.Entity);
        await this.unitOfWork.SaveChangesAsync();

        if (!this.blogSettings.NotifyOnNewComments || string.IsNullOrEmpty(this.blogSettings.NotifyOnNewCommentsEmail))
        {
            return;
        }

        var body = new StringBuilder();
        body.Append("Referer: ");
        body.AppendLine($"<a href=\"{HttpUtility.HtmlEncode(command.Referer)}#Comments\">{HttpUtility.HtmlEncode(command.Referer)}</a>");
        body.Append("<br /><br />Name: ");
        body.AppendLine(HttpUtility.HtmlEncode(command.Entity.Name));
        body.Append("<br />Email: ");
        body.AppendLine(HttpUtility.HtmlEncode(command.Entity.Email));
        body.Append("<br />Homepage: ");
        body.AppendLine(HttpUtility.HtmlEncode(command.Entity.Homepage));
        body.Append("<br /><br />Comment:<br />");
        body.AppendLine(HttpUtility.HtmlEncode(command.Entity.Comment).Replace("\r\n", "\n").Replace("\n", "<br />"));

        var message = new Message(
            new Recipient(this.blogSettings.NotifyOnNewCommentsEmail),
            this.blogSettings.NotifyOnNewCommentsSubject,
            body.ToString());

        if (!string.IsNullOrWhiteSpace(command.Entity.Email))
        {
            message.WithReplyTo(new Recipient(command.Entity.Email));
        }

        await this.notificationService.SendNotificationAsync(message);
    }
}