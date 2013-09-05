using System.Linq;
using MVCBlog.Core.Database;

namespace MVCBlog.Core.Commands
{
    public class DeleteImageCommandHandler : ICommandHandler<DeleteImageCommand>
    {
        private readonly IRepository repository;

        public DeleteImageCommandHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public void Handle(DeleteImageCommand command)
        {
            var entity = this.repository.Images.SingleOrDefault(e => e.Id == command.Id);

            if (entity != null)
            {
                this.repository.Images.Remove(entity);
                entity.DeleteData();

                this.repository.SaveChanges();
            }
        }
    }
}
