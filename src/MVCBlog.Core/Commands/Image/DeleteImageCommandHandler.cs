using System.Linq;
using System.Threading.Tasks;
using MVCBlog.Core.Database;
using System.Data.Entity;

namespace MVCBlog.Core.Commands
{
    public class DeleteImageCommandHandler : ICommandHandler<DeleteImageCommand>
    {
        private readonly IRepository repository;

        public DeleteImageCommandHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task HandleAsync(DeleteImageCommand command)
        {
            var entity = await this.repository.Images.SingleOrDefaultAsync(e => e.Id == command.Id);

            if (entity != null)
            {
                this.repository.Images.Remove(entity);
                entity.DeleteData();

                await this.repository.SaveChangesAsync();
            }
        }
    }
}
