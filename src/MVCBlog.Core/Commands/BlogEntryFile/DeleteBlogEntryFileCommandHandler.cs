using System.Linq;
using System.Threading.Tasks;
using MVCBlog.Core.Database;
using System.Data.Entity;

namespace MVCBlog.Core.Commands
{
    public class DeleteBlogEntryFileCommandHandler : ICommandHandler<DeleteBlogEntryFileCommand>
    {
        private readonly IRepository repository;

        public DeleteBlogEntryFileCommandHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task HandleAsync(DeleteBlogEntryFileCommand command)
        {
            var entity = await this.repository.BlogEntryFiles.SingleOrDefaultAsync(e => e.Id == command.Id);

            if (entity != null)
            {
                this.repository.BlogEntryFiles.Remove(entity);
                entity.DeleteData();

                await this.repository.SaveChangesAsync();
            }
        }
    }
}
