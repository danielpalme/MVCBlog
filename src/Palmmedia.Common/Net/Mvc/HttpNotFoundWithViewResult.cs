using System;
using System.Web.Mvc;

namespace Palmmedia.Common.Net.Mvc
{
    /// <summary>
    /// <see cref="System.Web.Mvc.ViewResult"/> which renders a 404-Error with a view.
    /// </summary>
    public class HttpNotFoundWithViewResult : ViewResult
    {
        /// <summary>
        /// HTTP 404 is the status code for Not Found
        /// </summary>
        private const int NotFoundCode = 404;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpNotFoundWithViewResult"/> class.
        /// </summary>
        public HttpNotFoundWithViewResult()
            : this(null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpNotFoundWithViewResult"/> class.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        public HttpNotFoundWithViewResult(string viewName)
            : this(viewName, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpNotFoundWithViewResult"/> class.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="statusDescription">The status description.</param>
        public HttpNotFoundWithViewResult(string viewName, string statusDescription)
            : base()
        {
            if (viewName == null)
            {
                throw new ArgumentNullException("viewName");
            }

            this.ViewName = viewName;
            this.StatusDescription = statusDescription;
        }

        /// <summary>
        /// Gets the status description.
        /// </summary>
        public string StatusDescription { get; private set; }

        /// <summary>
        /// When called by the action invoker, renders the view to the response.
        /// </summary>
        /// <param name="context">The context that the result is executed in.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="context"/> parameter is null.</exception>
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.StatusCode = NotFoundCode;
            if (this.StatusDescription != null)
            {
                context.HttpContext.Response.StatusDescription = this.StatusDescription;
            }

            base.ExecuteResult(context);
        }
    }
}
