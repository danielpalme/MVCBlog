using System.Data.Entity;
using System.Linq;
using MVCBlog.Core.Database;

namespace MVCBlog.Core.Commands
{
    public class DeleteBlogEntryCommandHandler : ICommandHandler<DeleteBlogEntryCommand>
    {
        private readonly IRepository repository;

        public DeleteBlogEntryCommandHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public void Handle(DeleteBlogEntryCommand command)
        {
            var entity = this.repository.BlogEntries
                .Include(b => b.BlogEntryFiles)
                .SingleOrDefault(e => e.Id == command.Id);

            if (entity != null)
            {
                foreach (var blogEntryFile in entity.BlogEntryFiles)
                {
                    blogEntryFile.DeleteData();
                }

                this.repository.BlogEntries.Remove(entity);

                this.repository.SaveChanges();
            }
        }
    }
}
