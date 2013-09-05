using System.Linq;
using MVCBlog.Core.Database;

namespace MVCBlog.Core.Commands
{
    public class DeleteBlogEntryFileCommandHandler : ICommandHandler<DeleteBlogEntryFileCommand>
    {
        private readonly IRepository repository;

        public DeleteBlogEntryFileCommandHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public void Handle(DeleteBlogEntryFileCommand command)
        {
            var entity = this.repository.BlogEntryFiles.SingleOrDefault(e => e.Id == command.Id);

            if (entity != null)
            {
                this.repository.BlogEntryFiles.Remove(entity);
                entity.DeleteData();

                this.repository.SaveChanges();
            }
        }
    }
}
