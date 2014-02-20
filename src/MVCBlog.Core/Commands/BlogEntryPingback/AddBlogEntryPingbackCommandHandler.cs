using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using MVCBlog.Core.Database;
using MVCBlog.Core.Service;

namespace MVCBlog.Core.Commands
{
    public class AddBlogEntryPingbackCommandHandler : ICommandHandler<AddBlogEntryPingbackCommand>
    {
        private readonly IRepository repository;
        private readonly IMessageService messageService;

        public AddBlogEntryPingbackCommandHandler(IRepository repository, IMessageService messageService)
        {
            this.repository = repository;
            this.messageService = messageService;
        }

        public async Task HandleAsync(AddBlogEntryPingbackCommand command)
        {
            this.repository.BlogEntryPingbacks.Add(command.Entity);
            await this.repository.SaveChangesAsync();

            string subject = ConfigurationManager.AppSettings["PingbackNotificationSubject"];
            string email = ConfigurationManager.AppSettings["adminEmail"];

            if (string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(email))
            {
                return;
            }

            var body = new StringBuilder();
            body.Append("Homepage: ");
            body.Append(command.Entity.Homepage);

            this.messageService.SendMessage(
                email,
                email,
                null,
                subject,
                body.ToString());
        }
    }
}
