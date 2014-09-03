using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using MVCBlog.Core.Commands;
using MVCBlog.Core.Database;
using MVCBlog.Core.Entities;
using MVCBlog.Website.Models.OutputModels.Blog;
using MvcSiteMapProvider.Web.Mvc.Filters;
using Palmmedia.Common.Linq;
using Palmmedia.Common.Net.Mvc;

namespace MVCBlog.Website.Controllers
{
    /// <summary>
    /// Controller for the blog.
    /// </summary>
    public partial class BlogController : Controller
    {
        /// <summary>
        /// Number of <see cref="BlogEntry">BlogEntries</see> per page.
        /// </summary>
        private const int ENTRIESPERPAGE = 8;

        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// The add blog entry comment command handler.
        /// </summary>
        private readonly ICommandHandler<AddBlogEntryCommentCommand> addBlogEntryCommentCommandHandler;

        /// <summary>
        /// The delete blog entry comment command hander.
        /// </summary>
        private readonly ICommandHandler<DeleteCommand<BlogEntryComment>> deleteBlogEntryCommentCommandHander;

        /// <summary>
        /// The delete blog entry command hander.
        /// </summary>
        private readonly ICommandHandler<DeleteBlogEntryCommand> deleteBlogEntryCommandHander;

        /// <summary>
        /// The update blog entry command handler.
        /// </summary>
        private readonly ICommandHandler<UpdateCommand<BlogEntry>> updateBlogEntryCommandHandler;

        /// <summary>
        /// The update blog entry file command handler.
        /// </summary>
        private readonly ICommandHandler<UpdateCommand<BlogEntryFile>> updateBlogEntryFileCommandHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogController" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="addBlogEntryCommentCommandHandler">The add blog entry comment command handler.</param>
        /// <param name="deleteBlogEntryCommentCommandHander">The delete blog entry comment command hander.</param>
        /// <param name="deleteBlogEntryCommandHander">The delete blog entry command hander.</param>
        /// <param name="updateBlogEntryCommandHandler">The update blog entry command handler.</param>
        /// <param name="updateBlogEntryFileCommandHandler">The update blog entry file command handler.</param>
        public BlogController(
            IRepository repository,
            ICommandHandler<AddBlogEntryCommentCommand> addBlogEntryCommentCommandHandler,
            ICommandHandler<DeleteCommand<BlogEntryComment>> deleteBlogEntryCommentCommandHander,
            ICommandHandler<DeleteBlogEntryCommand> deleteBlogEntryCommandHander,
            ICommandHandler<UpdateCommand<BlogEntry>> updateBlogEntryCommandHandler,
            ICommandHandler<UpdateCommand<BlogEntryFile>> updateBlogEntryFileCommandHandler)
        {
            this.repository = repository;

            this.addBlogEntryCommentCommandHandler = addBlogEntryCommentCommandHandler;
            this.deleteBlogEntryCommentCommandHander = deleteBlogEntryCommentCommandHander;
            this.deleteBlogEntryCommandHander = deleteBlogEntryCommandHander;
            this.updateBlogEntryCommandHandler = updateBlogEntryCommandHandler;
            this.updateBlogEntryFileCommandHandler = updateBlogEntryFileCommandHandler;
        }

        /// <summary>
        /// Shows all <see cref="BlogEntry">BlogEntries</see>.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="search">The search.</param>
        /// <param name="page">Number of the page.</param>
        /// <returns>A view showing all <see cref="BlogEntry">BlogEntries</see>.</returns>
        [ValidateInput(false)]
        public virtual ActionResult Index(string tag, string search, int? page)
        {
            var entries = this.GetAll(
                tag,
                search,
                new Paging(page.GetValueOrDefault(1), ENTRIESPERPAGE, PropertyResolver.GetPropertyName<BlogEntry>(b => b.PublishDate), SortDirection.Descending));

            var model = new IndexModel();
            model.Entries = entries.ToArray();
            model.TotalPages = (int)Math.Ceiling((double)entries.TotalNumberOfItems / ENTRIESPERPAGE);
            model.CurrentPage = page;
            model.Tag = tag;
            model.Search = search;

            return this.View(model);
        }

        /// <summary>
        /// Shows a single <see cref="BlogEntry"/>.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A view showing a single <see cref="BlogEntry"/>.</returns>
        [SiteMapTitle("Header")]
        public async virtual Task<ActionResult> Entry(string id)
        {
            var entry = await this.GetByHeader(id);

            if (entry == null)
            {
                return new HttpNotFoundWithViewResult(MVC.Shared.Views.NotFound);
            }

            if (!this.Request.IsAuthenticated)
            {
                entry.Visits++;
                await this.updateBlogEntryCommandHandler.HandleAsync(new UpdateCommand<BlogEntry>()
                {
                    Entity = entry
                });
            }

            return this.View(new BlogEntryDetail()
            {
                BlogEntry = entry,
                RelatedBlogEntries = await this.GetRelatedBlogEntries(entry)
            });
        }

        /// <summary>
        /// Adds a <see cref="Comment"/> to a <see cref="BlogEntry"/>.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="blogEntryComment">The comment.</param>
        /// <returns>A view showing a single <see cref="BlogEntry"/>.</returns>
        [SiteMapTitle("Header")]
        [Palmmedia.Common.Net.Mvc.SpamProtection(4)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async virtual Task<ActionResult> Entry(string id, [Bind(Include = "Name, Email, Homepage, Comment")] BlogEntryComment blogEntryComment)
        {
            var entry = await this.GetByHeader(id);

            if (entry == null)
            {
                return new HttpNotFoundResult();
            }

            this.ModelState.Remove("BlogEntryId");

            if (!ModelState.IsValid)
            {
                var errorModel = new BlogEntryDetail()
                {
                    BlogEntry = entry
                };

                if (this.Request.IsAjaxRequest())
                {
                    return this.PartialView(MVC.Blog.Views._CommentsControl, errorModel);
                }
                else
                {
                    errorModel.RelatedBlogEntries = await this.GetRelatedBlogEntries(entry);
                    return this.View(errorModel);
                }
            }

            blogEntryComment.AdminPost = this.Request.IsAuthenticated;
            blogEntryComment.BlogEntryId = entry.Id;
            entry.BlogEntryComments.Add(blogEntryComment);

            await this.addBlogEntryCommentCommandHandler.HandleAsync(new AddBlogEntryCommentCommand()
            {
                Entity = blogEntryComment
            });

            var model = new BlogEntryDetail()
            {
                BlogEntry = entry,
                HideNewCommentsForm = true
            };

            if (this.Request.IsAjaxRequest())
            {
                return this.PartialView(MVC.Blog.Views._CommentsControl, model);
            }
            else
            {
                model.RelatedBlogEntries = await this.GetRelatedBlogEntries(entry);
                return this.View(model);
            }
        }

        /// <summary>
        /// Shows a tag cloud.
        /// </summary>
        /// <returns>A view showing a tag cloud.</returns>
        [ChildActionOnly]
        [OutputCache(Duration = 3600)]
        public virtual ActionResult Tags()
        {
            var tags = this.repository.Tags
                .AsNoTracking()
                .OrderBy(t => t.Name)
                .ToArray();

            if (tags.Length > 0)
            {
                return this.PartialView(MVC.Shared.Views._TagListControl, tags);
            }
            else
            {
                return new EmptyResult();
            }
        }

        /// <summary>
        /// Shows the most populars the blog entries.
        /// </summary>
        /// <returns>A view showing the most populars the blog entries.</returns>
        [ChildActionOnly]
        [OutputCache(Duration = 3600)]
        public virtual ActionResult PopularBlogEntries()
        {
            var blogEntries = this.repository.BlogEntries
                .AsNoTracking()
                .OrderByDescending(b => b.Visits)
                .Take(5)
                .ToArray();

            if (blogEntries.Length > 0)
            {
                return this.PartialView(MVC.Shared.Views._PopularBlogEntriesControl, blogEntries);
            }
            else
            {
                return new EmptyResult();
            }
        }

        /// <summary>
        /// Returns an RSS-Feed of all <see cref="BlogEntry">BlogEntries</see>.
        /// </summary>
        /// <returns>RSS-Feed of all <see cref="BlogEntry">BlogEntries</see>.</returns>
        [OutputCache(CacheProfile = "Long")]
        public async virtual Task<ActionResult> Feed()
        {
            var entries = await this.repository.BlogEntries
                .Include(b => b.Tags)
                .AsNoTracking()
                .Where(b => b.Visible && b.PublishDate <= DateTime.Now)
                .OrderByDescending(b => b.PublishDate)
                .ToArrayAsync();

            string baseUrl = this.Request.Url.GetLeftPart(UriPartial.Authority);

            var feed = new SyndicationFeed(
                ConfigurationManager.AppSettings["BlogName"],
                ConfigurationManager.AppSettings["BlogDescription"],
                new Uri(this.Request.Url.GetLeftPart(UriPartial.Authority) + Url.Action(MVC.Blog.Feed())));

            feed.BaseUri = new Uri(baseUrl);

            if (entries.Any())
            {
                DateTime createdDate = entries.First().PublishDate;

                DateTime? lastModifiedDate = entries.OrderByDescending(e => e.Modified).First().Modified;
                if (lastModifiedDate.HasValue && lastModifiedDate.Value > createdDate)
                {
                    feed.LastUpdatedTime = lastModifiedDate.Value;
                }
                else
                {
                    feed.LastUpdatedTime = createdDate;
                }
            }

            var feedItems = new List<SyndicationItem>();

            foreach (var blogEntry in entries)
            {
                var syndicationItem = new SyndicationItem(
                    blogEntry.Header,
                    SyndicationContent.CreateXhtmlContent(blogEntry.ShortContent + blogEntry.Content),
                    new Uri(baseUrl + Url.Action(blogEntry.Url)),
                    blogEntry.Id.ToString(),
                    blogEntry.Modified.HasValue ? blogEntry.Modified.Value : blogEntry.PublishDate);

                if (!string.IsNullOrEmpty(blogEntry.Author))
                {
                    syndicationItem.Authors.Add(new SyndicationPerson() { Name = blogEntry.Author });
                }

                foreach (var tag in blogEntry.Tags)
                {
                    syndicationItem.Categories.Add(new SyndicationCategory(tag.Name));
                }

                feedItems.Add(syndicationItem);
            }

            feed.Items = feedItems;

            return new Palmmedia.Common.Net.Mvc.Feed.RssSyndicationFeedActionResult(feed);
        }

        /// <summary>
        /// Deletes the <see cref="BlogEntry"/> with the given id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A view showing all <see cref="BlogEntry">BlogEntries</see>.</returns>
        [Authorize]
        public async virtual Task<ActionResult> Delete(Guid id)
        {
            await this.deleteBlogEntryCommandHander.HandleAsync(new DeleteBlogEntryCommand()
            {
                Id = id
            });

            return this.RedirectToAction(MVC.Blog.Index());
        }

        /// <summary>
        /// Deletes the <see cref="Comment"/> with the given id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A view showing all <see cref="BlogEntry">BlogEntries</see>.</returns>
        [Authorize]
        public async virtual Task<ActionResult> DeleteComment(Guid id)
        {
            await this.deleteBlogEntryCommentCommandHander.HandleAsync(new DeleteCommand<BlogEntryComment>()
            {
                Id = id
            });

            return this.Redirect(this.Request.UrlReferrer.ToString());
        }

        /// <summary>
        /// Streams the <see cref="BlogEntryFile"/> with the given id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The <see cref="BlogEntryFile"/> as Download.</returns>
        public async virtual Task<ActionResult> Download(Guid id)
        {
            var blogEntryFile = await this.repository.BlogEntryFiles
                .AsNoTracking()
                .SingleOrDefaultAsync(b => b.Id == id);

            if (blogEntryFile == null)
            {
                return new HttpNotFoundWithViewResult(MVC.Shared.Views.NotFound);
            }

            blogEntryFile.Counter++;

            await this.updateBlogEntryFileCommandHandler.HandleAsync(new UpdateCommand<BlogEntryFile>()
            {
                Entity = blogEntryFile
            });

            var file = new FileContentResult(blogEntryFile.Data, "application/octet-stream");
            file.FileDownloadName = blogEntryFile.Name + "." + blogEntryFile.Extension;
            return file;
        }

        /// <summary>
        /// Shows the imprint.
        /// </summary>
        /// <returns>The imprint view.</returns>
        public virtual ActionResult Imprint()
        {
            return this.View();
        }

        /// <summary>
        /// Returns the <see cref="BlogEntry"/> with the given header.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <returns>
        /// The <see cref="BlogEntry"/> with the given header.
        /// </returns>
        private Task<BlogEntry> GetByHeader(string header)
        {
            var entry = this.repository.BlogEntries
                .Include(b => b.Tags)
                .Include(b => b.BlogEntryComments)
                .Include(b => b.BlogEntryFiles)
                .Include(b => b.BlogEntryPingbacks)
                .AsNoTracking()
                .Where(e => (e.Visible && e.PublishDate <= DateTime.Now) || this.Request.IsAuthenticated)
                .SingleOrDefaultAsync(e => e.HeaderUrl.Equals(header));

            return entry;
        }

        /// <summary>
        /// Returns all <see cref="BlogEntry">BlogEntries</see>.
        /// </summary>
        /// <param name="tag">The text of the tag.</param>
        /// <param name="search">The search.</param>
        /// <param name="paging">The <see cref="Paging"/>.</param>
        /// <returns>
        /// All <see cref="BlogEntry">BlogEntries</see>.
        /// </returns>
        private PagedResult<BlogEntry> GetAll(string tag, string search, Paging paging)
        {
            var query = this.repository.BlogEntries
                .Include(b => b.BlogEntryComments)
                .AsNoTracking()
                .Where(e => (e.Visible && e.PublishDate <= DateTime.Now) || this.Request.IsAuthenticated);

            if (!string.IsNullOrEmpty(tag))
            {
                query = query.Where(e => e.Tags.Count(t => t.Name.Equals(tag, StringComparison.OrdinalIgnoreCase)) > 0);
            }

            if (!string.IsNullOrEmpty(search))
            {
                foreach (var item in search.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Where(e => e.Header.Contains(item));
                }
            }

            return query.GetPagedResult(paging);
        }

        /// <summary>
        /// Returns related <see cref="BlogEntry">BlogEntries</see>.
        /// </summary>
        /// <param name="entry">The <see cref="BlogEntry"/>.</param>
        /// <returns>
        /// The related <see cref="BlogEntry">BlogEntries</see>.
        /// </returns>
        private Task<BlogEntry[]> GetRelatedBlogEntries(BlogEntry entry)
        {
            var tagIds = entry.Tags.Select(t => t.Id).ToArray();

            var query = this.repository.BlogEntries
                .AsNoTracking()
                .Where(e => e.Visible && e.PublishDate <= DateTime.Now && e.Id != entry.Id)
                .Where(e => e.Tags.Any(t => tagIds.Contains(t.Id)))
                .OrderByDescending(e => e.Tags.Count(t => tagIds.Contains(t.Id)))
                .ThenByDescending(e => e.Created)
                .Take(3)
                .ToArrayAsync();

            return query;
        }
    }
}
