using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCBlog.Core.Commands;
using MVCBlog.Core.Database;
using MVCBlog.Core.Entities;
using MVCBlog.Website.Models.OutputModels.Administration;

namespace MVCBlog.Website.Controllers
{
    /// <summary>
    /// Controller for the administration tasks.
    /// </summary>
    [Authorize]
    public partial class AdministrationController : Controller
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// The add  blog entry command handler.
        /// </summary>
        private readonly ICommandHandler<AddBlogEntryCommand> addBlogEntryCommandHandler;

        /// <summary>
        /// The update  blog entry command handler.
        /// </summary>
        private readonly ICommandHandler<UpdateBlogEntryCommand> updateBlogEntryCommandHandler;

        /// <summary>
        /// The add image command handler.
        /// </summary>
        private readonly ICommandHandler<AddImageCommand> addImageCommandHandler;

        /// <summary>
        /// The add or update blog entry file command handler.
        /// </summary>
        private readonly ICommandHandler<AddOrUpdateBlogEntryFileCommand> addOrUpdateBlogEntryFileCommandHandler;

        /// <summary>
        /// The delete blog entry file command handler.
        /// </summary>
        private readonly ICommandHandler<DeleteBlogEntryFileCommand> deleteBlogEntryFileCommandHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdministrationController" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="addOrUpdateBlogEntryCommandHandler">The add or update blog entry command handler.</param>
        /// <param name="addImageCommandHandler">The add image command handler.</param>
        /// <param name="addOrUpdateBlogEntryFileCommandHandler">The add or update blog entry file command handler.</param>
        /// <param name="deleteBlogEntryFileCommandHandler">The delete blog entry file command handler.</param>
        public AdministrationController(
            IRepository repository,
            ICommandHandler<AddBlogEntryCommand> addBlogEntryCommandHandler,
            ICommandHandler<UpdateBlogEntryCommand> updateBlogEntryCommandHandler,
            ICommandHandler<AddImageCommand> addImageCommandHandler,
            ICommandHandler<AddOrUpdateBlogEntryFileCommand> addOrUpdateBlogEntryFileCommandHandler,
            ICommandHandler<DeleteBlogEntryFileCommand> deleteBlogEntryFileCommandHandler)
        {
            this.repository = repository;
            this.addBlogEntryCommandHandler = addBlogEntryCommandHandler;
            this.updateBlogEntryCommandHandler = updateBlogEntryCommandHandler;
            this.addImageCommandHandler = addImageCommandHandler;
            this.addOrUpdateBlogEntryFileCommandHandler = addOrUpdateBlogEntryFileCommandHandler;
            this.deleteBlogEntryFileCommandHandler = deleteBlogEntryFileCommandHandler;
        }

        /// <summary>
        /// Shows an overview.
        /// </summary>
        /// <returns>View showing an overview.</returns>
        public virtual ActionResult Index()
        {
            return this.View();
        }

        /// <summary>
        /// Shows all downloads.
        /// </summary>
        /// <returns>View showing all downloads.</returns>
        public virtual ActionResult Statistics()
        {
            var minDate = DateTime.Now.Subtract(new TimeSpan(60, 0, 0, 0));

            var model = new MVCBlog.Website.Models.OutputModels.Administration.Downloads()
            {
                BlogEntries = this.repository.BlogEntries
                    .Include(b => b.BlogEntryFiles)
                    .AsNoTracking()
                    .OrderByDescending(f => f.PublishDate)
                    .ToArray(),

                FeedStatistics = this.repository.FeedStatistics
                    .AsNoTracking()
                    .OrderBy(f => f.Application)
                    .AsEnumerable()
                    .GroupBy(f => f.Created.Date)
                    .OrderBy(f => f.Key)
            };

            return this.View(model);
        }

        /// <summary>
        /// Show a single <see cref="BlogEntry"/> for editing.
        /// </summary>
        /// <param name="id">The Id of the <see cref="BlogEntry"/>.</param>
        /// <returns>A view showing a single <see cref="BlogEntry"/>.</returns>
        public virtual ActionResult EditBlogEntry(Guid? id)
        {
            var blogEntry = id.HasValue ? this.repository.BlogEntries
                    .Include(b => b.Tags)
                    .Include(b => b.BlogEntryFiles)
                    .AsNoTracking()
                .Single(b => b.Id == id.Value) : new BlogEntry()
                {
                    PublishDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0)
                };

            var model = new EditBlogEntry()
            {
                BlogEntry = blogEntry,
                Tags = this.repository.Tags.AsNoTracking().OrderBy(t => t.Name).ToArray(),
                Images = this.repository.Images.AsNoTracking().OrderByDescending(t => t.Created).ToArray(),
                IsUpdate = id.HasValue
            };

            return this.View(model);
        }

        /// <summary>
        /// Saves the changes applied to the <see cref="BlogEntry"/>.
        /// </summary>
        /// <param name="id">The Id of the <see cref="BlogEntry"/>.</param>
        /// <param name="blogEntry">The <see cref="BlogEntry"/>.</param>
        /// <param name="formValues">The form values.</param>
        /// <returns>A view showing a single <see cref="BlogEntry"/>.</returns>
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult EditBlogEntry(Guid? id, [Bind(Include = "Header, Author, ShortContent, Content, Visible")]BlogEntry blogEntry, FormCollection formValues)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(new EditBlogEntry()
                {
                    BlogEntry = blogEntry,
                    Tags = this.repository.Tags.AsNoTracking().OrderBy(t => t.Name).ToArray(),
                    Images = this.repository.Images.AsNoTracking().OrderByDescending(t => t.Created).ToArray(),
                    IsUpdate = id.HasValue
                });
            }

            if (id.HasValue)
            {
                var existingBlogEntry = this.repository.BlogEntries
                    .Include(b => b.Tags)
                    .Single(b => b.Id == id.Value);
                existingBlogEntry.Header = blogEntry.Header;
                existingBlogEntry.Author = blogEntry.Author;
                existingBlogEntry.ShortContent = blogEntry.ShortContent;
                existingBlogEntry.Content = blogEntry.Content;
                existingBlogEntry.PublishDate = blogEntry.PublishDate;
                existingBlogEntry.Visible = blogEntry.Visible;

                blogEntry = existingBlogEntry;
            }

            blogEntry.PublishDate = DateTime.Parse(formValues["PublishDate"]);

            var tags = formValues.AllKeys.Where(k => k.StartsWith("Tag") && !string.IsNullOrEmpty(formValues[k])).Select(k => formValues[k]);

            if (id.HasValue)
            {
                this.updateBlogEntryCommandHandler.Handle(new UpdateBlogEntryCommand()
                {
                    Entity = blogEntry,
                    Tags = tags
                });
            }
            else
            {
                this.addBlogEntryCommandHandler.Handle(new AddBlogEntryCommand()
                {
                    Entity = blogEntry,
                    Tags = tags
                });
            }

            return this.RedirectToAction(MVC.Administration.EditBlogEntry(blogEntry.Id));
        }

        /// <summary>
        /// Performs all pingback requests. All URLs mentioned in the BlogPost are notified.
        /// </summary>
        /// <param name="id">The Id of the <see cref="BlogEntry"/>.</param>
        /// <returns>A view showing the results of the pingback requests.</returns>
        [HttpPost]
        public virtual ActionResult PerformPingbacks(Guid id)
        {
            var blogEntry = this.repository.BlogEntries
                    .Single(b => b.Id == id);

            var pingbackResults = Palmmedia.Common.Net.PingBack.PingBackService.PerformPingBack(
                this.Request.Url.GetLeftPart(UriPartial.Authority) + Url.Action(blogEntry.Url, MVC.Blog.Name),
                blogEntry.Content);

            return this.View(MVC.Administration.Views.Pingback, pingbackResults);
        }

        /// <summary>
        /// Adds a file to a <see cref="BlogEntry"/>.
        /// </summary>
        /// <param name="id">The Id of the <see cref="BlogEntry"/>.</param>
        /// <param name="file">The uploaded file.</param>
        /// <returns>A view showing a single <see cref="BlogEntry"/>.</returns>
        [HttpPost]
        public virtual ActionResult FileUpload(Guid id, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                var content = new byte[file.ContentLength];
                file.InputStream.Read(content, 0, file.ContentLength);

                this.addOrUpdateBlogEntryFileCommandHandler.Handle(new AddOrUpdateBlogEntryFileCommand()
                {
                    BlogEntryId = id,
                    FileName = file.FileName,
                    Data = content
                });
            }

            return this.RedirectToAction(MVC.Administration.EditBlogEntry(id));
        }

        /// <summary>
        /// Files the delete.
        /// </summary>
        /// <param name="id">The file id.</param>
        /// <returns>A view showing a single <see cref="BlogEntry"/>.</returns>
        public virtual ActionResult FileDelete(Guid id)
        {
            this.deleteBlogEntryFileCommandHandler.Handle(new DeleteBlogEntryFileCommand()
            {
                Id = id
            });

            return this.RedirectToAction(MVC.Administration.EditBlogEntry(id));
        }

        /// <summary>
        /// Shows all <see cref="MVCBlog.Core.Entities.Image">Images</see>.
        /// </summary>
        /// <returns>A view showing all <see cref="MVCBlog.Core.Entities.Image">Images</see>.</returns>
        public virtual ActionResult Images()
        {
            var images = this.repository.Images
                .AsNoTracking()
                .OrderByDescending(t => t.Created)
                .ToArray();

            return this.View(images);
        }

        /// <summary>
        /// Upload an image
        /// </summary>
        /// <param name="file">The uploaded file.</param>
        /// <returns>A view showing all <see cref="MVCBlog.Core.Entities.Image">Images</see>.</returns>
        [HttpPost]
        public virtual ActionResult ImageUpload(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                var content = new byte[file.ContentLength];
                file.InputStream.Read(content, 0, file.ContentLength);

                this.addImageCommandHandler.Handle(new AddImageCommand()
                {
                    FileName = file.FileName,
                    Data = content
                });
            }

            return this.RedirectToAction(MVC.Administration.Images());
        }
    }
}
