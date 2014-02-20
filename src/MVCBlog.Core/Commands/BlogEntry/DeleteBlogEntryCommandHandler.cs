using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task HandleAsync(DeleteBlogEntryCommand command)
        {
            var entity = await this.repository.BlogEntries
                .Include(b => b.BlogEntryFiles)
                .SingleOrDefaultAsync(e => e.Id == command.Id);

            if (entity != null)
            {
                foreach (var blogEntryFile in entity.BlogEntryFiles)
                {
                    blogEntryFile.DeleteData();
                }

                this.repository.BlogEntries.Remove(entity);

                await this.repository.SaveChangesAsync();
            }
        }
    }
}
