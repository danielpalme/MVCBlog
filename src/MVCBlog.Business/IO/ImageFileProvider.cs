using Microsoft.AspNetCore.Hosting;

namespace MVCBlog.Business.IO
{
    public class ImageFileProvider : FileProviderBase, IImageFileProvider
    {
        private const string ImagesBaseDirectory = "wwwroot/blogimages";

        public ImageFileProvider(IHostingEnvironment hostingEnvironment)
            : base(hostingEnvironment, ImagesBaseDirectory)
        {
        }
    }
}