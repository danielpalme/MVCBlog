using MVCBlog.Data;

namespace MVCBlog.Business.Commands;

public class AddOrUpdateBlogEntryCommand
{
    public AddOrUpdateBlogEntryCommand(BlogEntry entity, IEnumerable<string> tags)
    {
        this.Entity = entity;
        this.Tags = tags;
    }

    public BlogEntry Entity { get; set; }

    public IEnumerable<string> Tags { get; set; }
}