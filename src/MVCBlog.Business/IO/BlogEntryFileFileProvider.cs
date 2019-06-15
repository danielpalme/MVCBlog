using Microsoft.AspNetCore.Hosting;

namespace MVCBlog.Business.IO
{
    public class BlogEntryFileFileProvider : FileProviderBase, IBlogEntryFileFileProvider
    {
        private const string FilesBaseDirectory = "wwwroot/blogfiles";

        public BlogEntryFileFileProvider(IHostingEnvironment hostingEnvironment)
            : base(hostingEnvironment, FilesBaseDirectory)
        {
        }
    }
}