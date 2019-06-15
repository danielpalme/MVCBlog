using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCBlog.Business;
using MVCBlog.Business.Commands;
using MVCBlog.Data;
using MVCBlog.Localization;
using MVCBlog.Web.Infrastructure.Excel;
using MVCBlog.Web.Infrastructure.Mvc;
using MVCBlog.Web.Infrastructure.Paging;
using MVCBlog.Web.Models.Administration;

namespace MVCBlog.Web.Controllers
{
    [Authorize]
    public class AdministrationController : Controller
    {
        private readonly EFUnitOfWork unitOfWork;

        private readonly ICommandHandler<AddOrUpdateBlogEntryCommand> addOrUpdateBlogEntryCommandHandler;

        private readonly ICommandHandler<DeleteBlogEntryCommand> deleteBlogEntryCommandHandler;

        private readonly ICommandHandler<AddOrUpdateBlogEntryFileCommand> addOrUpdateBlogEntryFileCommandHandler;

        private readonly ICommandHandler<DeleteBlogEntryFileCommand> deleteBlogEntryFileCommandHandler;

        private readonly ICommandHandler<AddImageCommand> addImageCommandHandler;

        private readonly ICommandHandler<DeleteImageCommand> deleteImageCommandHandler;

        private readonly UserManager<User> userManager;

        public AdministrationController(
            EFUnitOfWork unitOfWork,
            ICommandHandler<AddOrUpdateBlogEntryCommand> addOrUpdateBlogEntryCommandHandler,
            ICommandHandler<DeleteBlogEntryCommand> deleteBlogEntryCommandHandler,
            ICommandHandler<AddOrUpdateBlogEntryFileCommand> addOrUpdateBlogEntryFileCommandHandler,
            ICommandHandler<DeleteBlogEntryFileCommand> deleteBlogEntryFileCommandHandler,
            ICommandHandler<AddImageCommand> addImageCommandHandler,
            ICommandHandler<DeleteImageCommand> deleteImageCommandHandler,
            UserManager<User> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.addOrUpdateBlogEntryCommandHandler = addOrUpdateBlogEntryCommandHandler;
            this.deleteBlogEntryCommandHandler = deleteBlogEntryCommandHandler;
            this.addOrUpdateBlogEntryFileCommandHandler = addOrUpdateBlogEntryFileCommandHandler;
            this.deleteBlogEntryFileCommandHandler = deleteBlogEntryFileCommandHandler;
            this.addImageCommandHandler = addImageCommandHandler;
            this.deleteImageCommandHandler = deleteImageCommandHandler;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index(IndexViewModel model, Paging<BlogEntry> paging, bool? download)
        {
            if (paging.SortColumn == null)
            {
                paging.SetSortExpression(p => p.PublishDate);
                paging.SortDirection = SortDirection.Descending;
            }

            if (download.GetValueOrDefault())
            {
                paging.Top = int.MaxValue;
                paging.Skip = 0;
            }

            IQueryable<BlogEntry> query = this.unitOfWork.BlogEntries
                .AsNoTracking();

            if (model.SearchTerm != null)
            {
                query = query.Where(u => u.Header.Contains(model.SearchTerm));
            }

            model.BlogEntries = await query
                .GetPagedResultAsync(paging);

            if (download.GetValueOrDefault())
            {
                var document = this.GenerateDocument(model.BlogEntries);
                return this.File(document, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Posts.xlsx");
            }

            return this.View(model);
        }

        [Route("Blog/{year:int}/{month:int}/{day:int}/{id}/Edit")]
        [Route("[controller]/EditBlogEntry")]
        public async Task<ActionResult> EditBlogEntry(string id)
        {
            BlogEntry entry = null;

            if (id == null)
            {
                entry = new BlogEntry()
                {
                    AuthorId = this.userManager.GetUserId(this.User),
                    PublishDate = DateTimeOffset.UtcNow,
                    Tags = new Collection<BlogEntryTag>()
                };
            }
            else
            {
                entry = await this.GetByPermalink(id);

                if (entry == null)
                {
                    return this.NotFound();
                }
            }

            var model = new EditBlogEntryViewModel()
            {
                BlogEntry = entry
            };

            model.SelectedTagNames = model.BlogEntry.Tags.Select(t => t.Tag.Name).OrderBy(t => t).ToList();

            await this.SetTagsAndAuthors(model);

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Blog/{year:int}/{month:int}/{day:int}/{id}/Edit")]
        [Route("[controller]/EditBlogEntry")]
        public async Task<ActionResult> EditBlogEntry(string id, EditBlogEntryViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                await this.SetTagsAndAuthors(model);
                return this.View(model);
            }

            try
            {
                await this.addOrUpdateBlogEntryCommandHandler.HandleAsync(new AddOrUpdateBlogEntryCommand()
                {
                    Entity = model.BlogEntry,
                    Tags = model.SelectedTagNames
                });
            }
            catch (BusinessRuleException ex)
            {
                this.SetErrorMessage(ex.Message);
                await this.SetTagsAndAuthors(model);
                return this.View(model);
            }

            this.SetSuccessMessage(Resources.SavedSuccessfully);

            return this.Redirect($"/Blog/{model.BlogEntry.Url}/Edit");
        }

        public async Task<ActionResult> DeleteBlogEntry(Guid? id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.NotFound();
            }

            await this.deleteBlogEntryCommandHandler.HandleAsync(new DeleteBlogEntryCommand()
            {
                Id = id.Value
            });

            this.SetSuccessMessage(Resources.DeletedSuccessfully);

            return this.Redirect(this.Request.Headers["Referer"].ToString());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddBlogEntryFile(AddBlogEntryFileViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Redirect(this.Request.Headers["Referer"].ToString());
            }

            using (var memoryStream = new MemoryStream())
            {
                await model.File.CopyToAsync(memoryStream);
                byte[] file = memoryStream.ToArray();

                await this.addOrUpdateBlogEntryFileCommandHandler.HandleAsync(new AddOrUpdateBlogEntryFileCommand()
                {
                    BlogEntryId = model.BlogEntryId.Value,
                    Data = file,
                    FileName = model.File.FileName
                });
            }

            this.SetSuccessMessage(Resources.SavedSuccessfully);

            return this.Redirect(this.Request.Headers["Referer"].ToString());
        }

        public async Task<ActionResult> DeleteBlogEntryFile(Guid? id)
        {
            if (!id.HasValue)
            {
                return this.NotFound();
            }

            await this.deleteBlogEntryFileCommandHandler.HandleAsync(new DeleteBlogEntryFileCommand()
            {
                Id = id.Value
            });

            return this.Redirect(this.Request.Headers["Referer"].ToString());
        }

        public async Task<ActionResult> ImagesSelection()
        {
            var model = new ImagesSelectionViewModel();

            model.Images = await this.unitOfWork.Images
                .AsNoTracking()
                .OrderByDescending(i => i.CreatedOn)
                .Take(25)
                .ToListAsync();

            return this.PartialView(model);
        }

        public async Task<IActionResult> Downloads(DownloadViewModel model, Paging<BlogEntry> paging)
        {
            if (paging.SortColumn == null)
            {
                paging.SetSortExpression(p => p.PublishDate);
                paging.SortDirection = SortDirection.Descending;
            }

            IQueryable<BlogEntry> query = this.unitOfWork.BlogEntries
                .AsNoTracking()
                .Include(b => b.BlogEntryFiles);

            if (model.SearchTerm != null)
            {
                query = query.Where(u => u.Header.Contains(model.SearchTerm));
            }

            model.BlogEntries = await query
                .GetPagedResultAsync(paging);

            return this.View(model);
        }

        public async Task<IActionResult> Images(ImagesViewModel model, Paging<Image> paging)
        {
            if (paging.SortColumn == null)
            {
                paging.SetSortExpression(p => p.CreatedOn);
                paging.SortDirection = SortDirection.Descending;
            }

            IQueryable<Image> query = this.unitOfWork.Images
                .AsNoTracking();

            if (model.SearchTerm != null)
            {
                query = query.Where(u => u.Name.Contains(model.SearchTerm));
            }

            model.Images = await query
                .GetPagedResultAsync(paging);

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Images(ImagesViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction();
            }

            using (var ms = new MemoryStream())
            {
                await model.Image.OpenReadStream().CopyToAsync(ms);

                await this.addImageCommandHandler.HandleAsync(new AddImageCommand()
                {
                    FileName = model.Image.FileName,
                    Data = ms.ToArray()
                });
            }

            this.SetSuccessMessage(Resources.SavedSuccessfully);

            return this.RedirectToAction();
        }

        public async Task<ActionResult> DeleteImage(Guid? id)
        {
            if (!id.HasValue)
            {
                return this.NotFound();
            }

            await this.deleteImageCommandHandler.HandleAsync(new DeleteImageCommand()
            {
                Id = id.Value
            });

            this.SetSuccessMessage(Resources.DeletedSuccessfully);

            return this.RedirectToAction(nameof(this.Images));
        }

        public async Task<IActionResult> Users(UsersViewModel model, Paging<User> paging)
        {
            if (paging.SortColumn == null)
            {
                paging.SetSortExpression(p => p.LastName);
            }

            IQueryable<User> query = this.unitOfWork.Users
                .AsNoTracking();

            if (model.SearchTerm != null)
            {
                foreach (var part in model.SearchTerm.Replace(",", string.Empty).Split(' ', StringSplitOptions.RemoveEmptyEntries))
                {
                    if (part.Contains("@"))
                    {
                        query = query.Where(u => u.Email.Contains(part));
                    }
                    else
                    {
                        query = query
                           .Where(u => u.LastName.Contains(part)
                                || u.FirstName.Contains(part));
                    }
                }
            }

            model.Users = await query
                .GetPagedResultAsync(paging);

            return this.View(model);
        }

        private async Task SetTagsAndAuthors(EditBlogEntryViewModel model)
        {
            model.AllTags = await this.unitOfWork.Tags
                .AsNoTracking()
                .OrderBy(t => t.Name)
                .ToListAsync();

            model.Authors = await this.unitOfWork.Users
                .AsNoTracking()
                .OrderBy(t => t.LastName)
                .ThenBy(t => t.FirstName)
                .ToListAsync();

            if (model.BlogEntry.BlogEntryFiles == null)
            {
                model.BlogEntry.BlogEntryFiles = await this.unitOfWork.BlogEntryFiles
                    .AsNoTracking()
                    .Where(b => b.BlogEntryId == model.BlogEntry.Id)
                    .ToListAsync();
            }
        }

        private byte[] GenerateDocument(IEnumerable<BlogEntry> blogEntries)
        {
            var columns = new[]
                {
                    blogEntries.CreateColumn(Resources.Header, i => i.Header),
                    blogEntries.CreateColumn(Resources.PublishDate, i => i.PublishDate.DateTime.ToShortDateString()),
                    blogEntries.CreateColumn(Resources.UpdateDate, i => i.UpdateDate.DateTime.ToShortDateString()),
                    blogEntries.CreateColumn(Resources.Visits, i => i.Visits)
                };

            var document = GenericExcelGenerator.GenerateDocument(
                true,
                blogEntries.CreateSheet("Posts", columns));

            return document;
        }

        /// <summary>
        /// Returns the <see cref="BlogEntry"/> with the given header.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <returns>
        /// The <see cref="BlogEntry"/> with the given header.
        /// </returns>
        private async Task<BlogEntry> GetByPermalink(string header)
        {
            var entry = await this.unitOfWork.BlogEntries
                .Include(b => b.Tags)
                .ThenInclude(b => b.Tag)
                .Include(b => b.BlogEntryFiles)
                .AsNoTracking()
                .SingleOrDefaultAsync(e => e.Permalink.Equals(header));

            return entry;
        }
    }
}