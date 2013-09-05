using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using MVCBlog.Core.Entities;

namespace MVCBlog.Core.Database
{
    /// <summary>
    /// Interface for database access.
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Gets the <see cref="BlogEntry">BlogEntries</see>.
        /// </summary>
        IDbSet<BlogEntry> BlogEntries { get; }

        /// <summary>
        /// Gets the <see cref="BlogEntryComment">BlogEntryComments</see>.
        /// </summary>
        IDbSet<BlogEntryComment> BlogEntryComments { get; }

        /// <summary>
        /// Gets the <see cref="BlogEntryPingback">BlogEntryPingbacks</see>.
        /// </summary>
        IDbSet<BlogEntryPingback> BlogEntryPingbacks { get; }

        /// <summary>
        /// Gets the <see cref="BlogEntryFile">BlogEntryFiles</see>.
        /// </summary>
        IDbSet<BlogEntryFile> BlogEntryFiles { get; }

        /// <summary>
        /// Gets the <see cref="Image">Images</see>.
        /// </summary>
        IDbSet<Image> Images { get; }

        /// <summary>
        /// Gets the <see cref="Tag">Tags</see>.
        /// </summary>
        IDbSet<Tag> Tags { get; }

        /// <summary>
        /// Gets the <see cref="FeedStatistic">FeedStatistics</see>.
        /// </summary>
        IDbSet<FeedStatistic> FeedStatistics { get; }

        DbSet<T> Set<T>() where T : class;

        DbEntityEntry Entry(object entity);

        /// <summary>
        /// Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>
        /// The number of objects written to the underlying database.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">Thrown if the context has been disposed.</exception>
        int SaveChanges();
    }
}
