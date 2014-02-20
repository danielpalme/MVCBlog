using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using MVCBlog.Core.Commands;
using Palmmedia.Common;

namespace MVCBlog.Website
{
    /// <summary>
    /// <see cref="IHttpModule"/> counting the number of readers of a feed.
    /// </summary>
    public class FeedSubscriberCounterModule : IHttpModule
    {
        /// <summary>
        /// The absolute path of your feed.
        /// If your feed is located at 'http://www.mydomain.xyz/Blog/Feed' the absolute path is '/Blog/Feed'
        /// </summary>
        private const string FEEDPATH = "/Blog/Feed";

        /// <summary>
        /// The pattern the useragent of a feed aggregators must match.
        /// </summary>
        private const string AGGREGATORPATTERN = @".+?(\d*).?(?>subscribers|readers|users).?(\d*).*";

        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpApplication"/> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application</param>
        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(this.Context_BeginRequest);
        }

        /// <summary>
        /// Disposes of the resources (other than memory) used by the module that implements <see cref="T:System.Web.IHttpModule"/>.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Handles the BeginRequest event of the <see cref="HttpApplication"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Context_BeginRequest(object sender, EventArgs e)
        {
            var request = HttpContext.Current.Request;

            if (request.Url.AbsolutePath.EndsWith(FEEDPATH, StringComparison.OrdinalIgnoreCase))
            {
                this.RegisterRequest(request);
            }
        }

        /// <summary>
        /// Analyses the request to determinate whether the request's origin is a single user or a feed aggregator.
        /// </summary>
        /// <param name="request">The request.</param>
        private void RegisterRequest(HttpRequest request)
        {
            var match = Regex.Match(request.UserAgent ?? string.Empty, AGGREGATORPATTERN, RegexOptions.Compiled);

            string application = request.Browser.Browser + " " + request.Browser.Version;

            if (application.StartsWith("Unknown", StringComparison.OrdinalIgnoreCase))
            {
                application = Regex.Match(request.UserAgent, @"^(?>\w|-)*", RegexOptions.Compiled).Value;
            }

            if (match.Success)
            {
                string numberOfSubscribers = match.Groups[1].Value;

                if (string.IsNullOrEmpty(numberOfSubscribers))
                {
                    numberOfSubscribers = match.Groups[2].Value;
                }

                var addOrUpdateFeedAggregatorFeedUserCommandCommandHandler = DependencyResolver.Current.GetService<ICommandHandler<AddOrUpdateFeedAggregatorFeedUserCommand>>();
                addOrUpdateFeedAggregatorFeedUserCommandCommandHandler.HandleAsync(new AddOrUpdateFeedAggregatorFeedUserCommand()
                {
                    Application = application,
                    Users = int.Parse(numberOfSubscribers)
                });
            }
            else
            {
                var addOrUpdateSingleFeedUserCommandHandler = DependencyResolver.Current.GetService<ICommandHandler<AddOrUpdateSingleFeedUserCommand>>();
                addOrUpdateSingleFeedUserCommandHandler.HandleAsync(new AddOrUpdateSingleFeedUserCommand()
                {
                    Application = application,
                    Identifier = (request.UserHostAddress + request.UserAgent).EncryptSha1()
                });
            }
        }
    }
}
