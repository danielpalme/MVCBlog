using MVCBlog.Business.IO;
using MVCBlog.Data;

namespace MVCBlog.Business.Commands;

public class AddImageCommandHandler : ICommandHandler<AddImageCommand>
{
    private readonly EFUnitOfWork unitOfWork;

    private readonly IImageFileProvider fileProvider;

    public AddImageCommandHandler(EFUnitOfWork unitOfWork, IImageFileProvider fileProvider)
    {
        this.unitOfWork = unitOfWork;
        this.fileProvider = fileProvider;
    }

    public async Task HandleAsync(AddImageCommand command)
    {
        string fileName = command.FileName.Replace('/', '\\');
        fileName = fileName.Substring(fileName.IndexOf('\\') + 1);

        string extension = fileName.Substring(fileName.LastIndexOf('.') + 1);

        var image = new Image(fileName);

        this.unitOfWork.Images.Add(image);

        await this.fileProvider.AddFileAsync($"{image.Id}.{extension}", command.Data);

        await this.unitOfWork.SaveChangesAsync();
    }
}