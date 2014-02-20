using System;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using CookComputing.XmlRpc;
using MVCBlog.Core.Commands;
using MVCBlog.Core.Database;
using MVCBlog.Core.Entities;
using Palmmedia.Common.Net.PingBack;

namespace MVCBlog.Website
{
    /// <summary>
    /// <see cref="XmlRpcService"/> handling pingback requests.
    /// </summary>
    public class PingbackHandler : XmlRpcService
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private IRepository repository;

        /// <summary>
        /// The AddBlogEntryPingbackCommandHandler.
        /// </summary>
        private ICommandHandler<AddBlogEntryPingbackCommand> addBlogEntryPingbackCommandHandler;

        /// <summary>
        /// The blogEntry.
        /// </summary>
        private BlogEntry blogEntry;

        #region Properties

        /// <summary>
        /// Gets or sets the repository.
        /// </summary>
        public IRepository Repository
        {
            get
            {
                if (this.repository == null)
                {
                    this.repository = DependencyResolver.Current.GetService<IRepository>();
                }

                return this.repository;
            }

            set
            {
                this.repository = value;
            }
        }

        /// <summary>
        /// Gets or sets the AddBlogEntryPingbackCommandHandler.
        /// </summary>
        public ICommandHandler<AddBlogEntryPingbackCommand> AddBlogEntryPingbackCommandHandler
        {
            get
            {
                if (this.addBlogEntryPingbackCommandHandler == null)
                {
                    this.addBlogEntryPingbackCommandHandler = DependencyResolver.Current.GetService<ICommandHandler<AddBlogEntryPingbackCommand>>();
                }

                return this.addBlogEntryPingbackCommandHandler;
            }

            set
            {
                this.addBlogEntryPingbackCommandHandler = value;
            }
        }

        #endregion

        /// <summary>
        /// Receives a pingback request.
        /// </summary>
        /// <param name="sourceUri">The source URI.</param>
        /// <param name="targetUri">The target URI.</param>
        /// <returns>The success message.</returns>
        [XmlRpcMethod("pingback.ping")]
        public string Pingback(string sourceUri, string targetUri)
        {
            PingBackService.ValidatePingbackRequest(
                sourceUri,
                targetUri,
                this.IsTargetPingbackEnabled,
                this.DoesPingbackAlreadyExist,
                this.RegisterPingback);

            return "Your ping request has been received successfully.";
        }

        /// <summary>
        /// Checks whether the target URI can receive pingbacks.
        /// </summary>
        /// <param name="targetUri">The target URI.</param>
        /// <returns><c>true</c> if target URI points to a <see cref="BlogEntry"/>, otherwise <c>false</c>.</returns>
        private bool IsTargetPingbackEnabled(string targetUri)
        {
            var match = Regex.Match(targetUri, ConfigurationManager.AppSettings["PingbackPattern"], RegexOptions.IgnoreCase);

            if (match.Success)
            {
                this.blogEntry = this.repository.BlogEntries
                    .AsNoTracking()
                    .Where(e => (e.Visible && e.PublishDate <= DateTime.Now))
                    .SingleOrDefault(e => e.HeaderUrl.Equals(match.Groups[1].Value));

                return this.blogEntry != null;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if a pingback from the source URI has already been registered.
        /// </summary>
        /// <param name="sourceUri">The source URI.</param>
        /// <returns><c>true</c> if pingback does already exist <see cref="BlogEntry"/>, otherwise <c>false</c>.</returns>
        private bool DoesPingbackAlreadyExist(string sourceUri)
        {
            return this.repository
                .BlogEntryPingbacks
                .Any(c => c.Homepage.Equals(sourceUri) && c.BlogEntry.Id == this.blogEntry.Id);
        }

        /// <summary>
        /// Registers the pingback.
        /// </summary>
        /// <param name="sourceUri">The source URI.</param>
        private void RegisterPingback(string sourceUri)
        {
            var blogEntryPingback = new BlogEntryPingback();

            blogEntryPingback.Homepage = sourceUri;
            blogEntryPingback.BlogEntryId = this.blogEntry.Id;
            this.blogEntry.BlogEntryPingbacks.Add(blogEntryPingback);

            this.AddBlogEntryPingbackCommandHandler.HandleAsync(new AddBlogEntryPingbackCommand()
            {
                Entity = blogEntryPingback
            });
        }
    }
}