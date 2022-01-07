using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using MVCBlog.Data;
using MVCBlog.Localization;

namespace MVCBlog.Business.Commands;

public class AddOrUpdateBlogEntryCommandHandler :
    ICommandHandler<AddOrUpdateBlogEntryCommand>
{
    private readonly EFUnitOfWork unitOfWork;

    public AddOrUpdateBlogEntryCommandHandler(EFUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(AddOrUpdateBlogEntryCommand command)
    {
        bool blogEntryWithSamePermalink = this.unitOfWork.BlogEntries
            .Any(b => b.Id != command.Entity.Id && b.Permalink == command.Entity.Permalink);

        if (blogEntryWithSamePermalink)
        {
            throw new BusinessRuleException(string.Format(Resources.PermalinkInUse, command.Entity.Permalink));
        }

        var blogEntry = this.unitOfWork.BlogEntries
            .Include(b => b.Tags!)
            .ThenInclude(b => b.Tag)
            .SingleOrDefault(b => b.Id == command.Entity.Id);

        if (blogEntry == null)
        {
            blogEntry = command.Entity;
            blogEntry.Permalink = Regex.Replace(
                blogEntry.Header.ToLowerInvariant().Replace(" - ", "-").Replace(" ", "-"),
                "[^\\w^-]",
                string.Empty);

            blogEntry.UpdateDate = blogEntry.CreatedOn;

            this.unitOfWork.BlogEntries.Add(blogEntry);
        }
        else
        {
            blogEntry.UpdateDate = DateTimeOffset.UtcNow;

            blogEntry.Header = command.Entity.Header;
            blogEntry.Permalink = command.Entity.Permalink;
            blogEntry.ShortContent = command.Entity.ShortContent;
            blogEntry.Content = command.Entity.Content;
            blogEntry.AuthorId = command.Entity.AuthorId;
            blogEntry.PublishDate = command.Entity.PublishDate;
            blogEntry.Visible = command.Entity.Visible;
        }

        await this.AddTagsAsync(blogEntry, command.Tags);

        await this.unitOfWork.SaveChangesAsync();
    }

    /// <summary>
    /// Adds the tags to the given <see cref="BlogEntry"/>.
    /// </summary>
    /// <param name="entry">The entry.</param>
    /// <param name="tags">The tags.</param>
    private async Task AddTagsAsync(BlogEntry entry, IEnumerable<string> tags)
    {
        var existingTags = await this.unitOfWork.Tags.ToListAsync();

        if (entry.Tags == null)
        {
            entry.Tags = new Collection<BlogEntryTag>();
        }

        foreach (var tag in entry.Tags.Where(t => !tags.Contains(t.Tag!.Name)).ToArray())
        {
            entry.Tags.Remove(tag);
        }

        foreach (var tag in tags.Where(t => !entry.Tags.Select(et => et.Tag!.Name).Contains(t)).ToArray())
        {
            var existingTag = existingTags.SingleOrDefault(t => t.Name.Equals(tag, StringComparison.OrdinalIgnoreCase));

            if (existingTag == null)
            {
                existingTag = new Tag(tag);
                existingTags.Add(existingTag);

                this.unitOfWork.Tags.Add(existingTag);
            }

            entry.Tags.Add(new BlogEntryTag()
            {
                BlogEntryId = entry.Id,
                TagId = existingTag.Id
            });
        }
    }
}