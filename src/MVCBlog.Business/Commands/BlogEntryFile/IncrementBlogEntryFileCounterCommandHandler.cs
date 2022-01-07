using Microsoft.EntityFrameworkCore;
using MVCBlog.Data;

namespace MVCBlog.Business.Commands;

public class IncrementBlogEntryFileCounterCommandHandler :
    ICommandHandler<IncrementBlogEntryFileCounterCommand>
{
    private readonly EFUnitOfWork unitOfWork;

    public IncrementBlogEntryFileCounterCommandHandler(EFUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(IncrementBlogEntryFileCounterCommand command)
    {
        await this.unitOfWork.Database.ExecuteSqlInterpolatedAsync(
            $"UPDATE [BlogEntryFiles] SET [Counter] = [Counter] + 1 WHERE [Id] = {command.Id}");
    }
}