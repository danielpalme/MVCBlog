using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using MVCBlog.Core.Database;
using MVCBlog.Core.Service;

namespace MVCBlog.Core.Commands
{
    public class AddBlogEntryCommentCommandHandler : ICommandHandler<AddBlogEntryCommentCommand>
    {
        private readonly IRepository repository;
        private readonly IMessageService messageService;

        public AddBlogEntryCommentCommandHandler(IRepository repository, IMessageService messageService)
        {
            this.repository = repository;
            this.messageService = messageService;
        }

        public async Task HandleAsync(AddBlogEntryCommentCommand command)
        {
            this.repository.BlogEntryComments.Add(command.Entity);
            await this.repository.SaveChangesAsync();

            string subject = ConfigurationManager.AppSettings["CommentNotificationSubject"];
            string email = ConfigurationManager.AppSettings["adminEmail"];

            if (string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(email))
            {
                return;
            }

            var body = new StringBuilder();
            body.Append("Name: ");
            body.Append(command.Entity.Name);
            body.Append("\nEmail: ");
            body.Append(command.Entity.Email);
            body.Append("\nHomepage: ");
            body.Append(command.Entity.Homepage);
            body.Append("\nComment: ");
            body.Append(command.Entity.Comment);

            this.messageService.SendMessage(
                email,
                email,
                command.Entity.Email,
                subject,
                body.ToString());
        }
    }
}
