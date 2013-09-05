using System.Linq;
using MVCBlog.Core.Database;

namespace MVCBlog.Core.Commands
{
    public class DeleteCommandHandler<T> : ICommandHandler<DeleteCommand<T>> where T : MVCBlog.Core.Entities.EntityBase
    {
        private readonly IRepository repository;

        public DeleteCommandHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public void Handle(DeleteCommand<T> command)
        {
            var entity = this.repository.Set<T>().SingleOrDefault(e => e.Id == command.Id);

            if (entity != null)
            {
                this.repository.Set<T>().Remove(entity);
                this.repository.SaveChanges();
            }
        }
    }
}
