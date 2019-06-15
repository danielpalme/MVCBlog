using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MVCBlog.Data;

namespace MVCBlog.Business.Commands
{
    public class IncrementBlogEntryVisitsCommandHandler :
        ICommandHandler<IncrementBlogEntryVisitsCommand>
    {
        private readonly EFUnitOfWork unitOfWork;

        public IncrementBlogEntryVisitsCommandHandler(EFUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(IncrementBlogEntryVisitsCommand command)
        {
            await this.unitOfWork.Database.ExecuteSqlCommandAsync(
                "UPDATE [BlogEntries] SET [Visits] = [Visits] + 1 WHERE [Id] = @Id",
                new SqlParameter("@Id", command.Id));
        }
    }
}
