using MVCBlog.Data;

namespace MVCBlog.Web.Models.Administration;

public class EditBlogEntryViewModel
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public EditBlogEntryViewModel()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
    }

    public EditBlogEntryViewModel(BlogEntry blogEntry)
    {
        this.BlogEntry = blogEntry;
    }

    public BlogEntry BlogEntry { get; set; }

    public List<string> SelectedTagNames { get; set; } = new List<string>();

    public List<Tag> AllTags { get; set; } = new List<Tag>();

    public List<User> Authors { get; set; } = new List<User>();
}
