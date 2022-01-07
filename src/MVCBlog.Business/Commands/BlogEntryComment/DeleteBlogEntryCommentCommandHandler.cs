using Microsoft.EntityFrameworkCore;
using MVCBlog.Data;

namespace MVCBlog.Business.Commands;

public class DeleteBlogEntryCommentCommandHandler : ICommandHandler<DeleteBlogEntryCommentCommand>
{
    private readonly EFUnitOfWork unitOfWork;

    public DeleteBlogEntryCommentCommandHandler(EFUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(DeleteBlogEntryCommentCommand command)
    {
        var entity = await this.unitOfWork.BlogEntryComments.SingleOrDefaultAsync(e => e.Id == command.Id);

        if (entity != null)
        {
            this.unitOfWork.BlogEntryComments.Remove(entity);

            await this.unitOfWork.SaveChangesAsync();
        }
    }
}