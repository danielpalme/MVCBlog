using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MVCBlog.Business.Email;
using MVCBlog.Data;

namespace MVCBlog.Business.Commands
{
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
            body.Append("Name: ");
            body.AppendLine(command.Entity.Name);
            body.Append("<br />Email: ");
            body.AppendLine(command.Entity.Email);
            body.Append("<br />Homepage: ");
            body.AppendLine(command.Entity.Homepage);
            body.Append("<br /><br />Comment:<br />");
            body.AppendLine(command.Entity.Comment);

            var message = new Message(
                new Recipient(this.blogSettings.NotifyOnNewCommentsEmail),
                this.blogSettings.NotifyOnNewCommentsSubject,
                body.ToString());

            await this.notificationService.SendNotificationAsync(message);
        }
    }
}
