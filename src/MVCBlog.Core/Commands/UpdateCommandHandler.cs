using MVCBlog.Core.Database;

namespace MVCBlog.Core.Commands
{
    public class UpdateCommandHandler<T> : ICommandHandler<UpdateCommand<T>> where T : MVCBlog.Core.Entities.EntityBase
    {
        private readonly IRepository repository;

        public UpdateCommandHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public void Handle(UpdateCommand<T> command)
        {
            var entry = this.repository.Entry(command.Entity);

            if (entry.State == System.Data.EntityState.Detached)
            {
                this.repository.Set<T>().Attach(command.Entity);
            }

            entry.State = System.Data.EntityState.Modified;

            this.repository.SaveChanges();
        }
    }
}
