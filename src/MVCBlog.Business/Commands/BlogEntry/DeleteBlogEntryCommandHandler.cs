using Microsoft.EntityFrameworkCore;
using MVCBlog.Business.IO;
using MVCBlog.Data;

namespace MVCBlog.Business.Commands;

public class DeleteBlogEntryCommandHandler : ICommandHandler<DeleteBlogEntryCommand>
{
    private readonly EFUnitOfWork unitOfWork;

    private readonly IBlogEntryFileFileProvider fileProvider;

    public DeleteBlogEntryCommandHandler(EFUnitOfWork unitOfWork, IBlogEntryFileFileProvider fileProvider)
    {
        this.unitOfWork = unitOfWork;
        this.fileProvider = fileProvider;
    }

    public async Task HandleAsync(DeleteBlogEntryCommand command)
    {
        var entity = await this.unitOfWork.BlogEntries
            .Include(b => b.BlogEntryFiles)
            .SingleOrDefaultAsync(e => e.Id == command.Id);

        if (entity != null)
        {
            this.unitOfWork.BlogEntries.Remove(entity);

            await this.unitOfWork.SaveChangesAsync();

            foreach (var file in entity.BlogEntryFiles!)
            {
                string extension = file.Name.Substring(file.Name.LastIndexOf('.') + 1);
                await this.fileProvider.DeleteFileAsync($"{file.Id}.{extension}");
            }
        }
    }
}