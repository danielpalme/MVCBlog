using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using MVCBlog.Core.Entities;

namespace MVCBlog.Core.Database
{
    /// <summary>
    /// Database Context based on EntityFramework (CodeFirst).
    /// </summary>
    public class DatabaseContext : DbContext, IRepository
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(DatabaseContext));

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseContext"/> class.
        /// </summary>
        public DatabaseContext()
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        /// <summary>
        /// Gets or sets the <see cref="BlogEntry">BlogEntries</see>.
        /// </summary>
        public IDbSet<BlogEntry> BlogEntries { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="BlogEntryComment">BlogEntryComments</see>.
        /// </summary>
        public IDbSet<BlogEntryComment> BlogEntryComments { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="BlogEntryPingback">BlogEntryPingbacks</see>.
        /// </summary>
        public IDbSet<BlogEntryPingback> BlogEntryPingbacks { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="BlogEntryFile">BlogEntryFiles</see>.
        /// </summary>
        public IDbSet<BlogEntryFile> BlogEntryFiles { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Image">Images</see>.
        /// </summary>
        public IDbSet<Image> Images { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Tag">Tags</see>.
        /// </summary>
        public IDbSet<Tag> Tags { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="FeedStatistic">FeedStatistics</see>.
        /// </summary>
        public IDbSet<FeedStatistic> FeedStatistics { get; set; }

        /// <summary>
        /// Asynchronously saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous save operation.
        /// The task result contains the number of objects written to the underlying database.
        /// </returns>
        /// <remarks>
        /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
        /// that any asynchronous operations have completed before calling another method on this context.
        /// </remarks>
        public override async Task<int> SaveChangesAsync()
        {
            // Set modification date if an entity has been modified
            foreach (var entity in this.ChangeTracker.Entries<EntityBase>().Where(e => e.State == System.Data.Entity.EntityState.Modified))
            {
                entity.Entity.Modified = DateTime.Now;
            }

            try
            {
                return await base.SaveChangesAsync();
            }
            catch (DbEntityValidationException ex)
            {
                var errors = ex.EntityValidationErrors
                        .Where(e => !e.IsValid)
                        .Select(e => e.Entry.Entity.GetType().Name + " - Errors: " + string.Join(", ", e.ValidationErrors.Select(v => v.PropertyName + ": " + v.ErrorMessage)));

                string errorText = string.Join("\r\n", errors);
                Logger.Error(string.Format("Saving to database failed (Errors: {0})", errorText), ex);
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error("Saving to database failed", ex);
                throw;
            }
        }
    }
}
