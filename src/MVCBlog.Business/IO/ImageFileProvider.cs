using Microsoft.Extensions.Hosting;

namespace MVCBlog.Business.IO;

public class ImageFileProvider : FileProviderBase, IImageFileProvider
{
    private const string ImagesBaseDirectory = "wwwroot/blogimages";

    public ImageFileProvider(IHostEnvironment hostEnvironment)
        : base(hostEnvironment, ImagesBaseDirectory)
    {
    }
}