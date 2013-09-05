using System;
using System.Web;
using System.Web.Mvc;

namespace Palmmedia.Common.Net.Mvc
{
    /// <summary>
    /// Verifies that the referrer of a request matches the current URL or contains a certain <see cref="string"/>.
    /// Used in POST scenarios to ensure that the request has been triggered from a specific URL.
    /// </summary>
    public sealed class ReferrerAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReferrerAuthorizeAttribute"/> class.
        /// </summary>
        public ReferrerAuthorizeAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferrerAuthorizeAttribute"/> class.
        /// </summary>
        /// <param name="valueUrlMustContain">The <see cref="string"/> the referrer URL must contain.</param>
        public ReferrerAuthorizeAttribute(string valueUrlMustContain)
        {
            this.ValueUrlMustContain = valueUrlMustContain;
        }

        /// <summary>
        /// Gets a <see cref="string"/> the referrer URL must contain.
        /// </summary>
        public string ValueUrlMustContain { get; private set; }

        /// <summary>
        /// Called when a process requests authorization.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            var request = filterContext.RequestContext.HttpContext.Request;

            if (request.UrlReferrer == null)
            {
                throw new HttpException("Invalid form submission. Referrer is not set.");
            }

            if (this.ValueUrlMustContain != null)
            {
                if (!request.IsLocal && !request.UrlReferrer.ToString().Contains(this.ValueUrlMustContain))
                {
                    throw new HttpException(string.Format("Invalid form submission. Referrer does not contain '{0}' (Referrer: {1}, Parameters: {2}).", this.ValueUrlMustContain, request.UrlReferrer, request.Params.ToString()));
                }
            }
            else if (!request.UrlReferrer.Equals(request.Url))
            {
                throw new HttpException(string.Format("Invalid form submission. Referrer does not match current URL (Referrer: {0}, URL: {1}, Parameters: {2}).", request.UrlReferrer, request.Url, request.Params.ToString()));
            }
        }
    }
}