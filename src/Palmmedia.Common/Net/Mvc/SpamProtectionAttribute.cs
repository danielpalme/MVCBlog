using System;
using System.Web;
using System.Web.Mvc;

namespace Palmmedia.Common.Net.Mvc
{
    /// <summary>
    /// Verifies that a form has not been submitted in a too short timespan.
    /// Used in POST scenarios to ensure that no spam-bot is performing the request.
    /// </summary>
    public sealed class SpamProtectionAttribute : FilterAttribute, IAuthorizationFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpamProtectionAttribute"/> class.
        /// </summary>
        public SpamProtectionAttribute()
            : this(1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpamProtectionAttribute"/> class.
        /// </summary>
        /// <param name="timespan">The he minimum timespan between GET- and POST-request.</param>
        public SpamProtectionAttribute(int timespan)
        {
            this.Timespan = timespan;
        }

        /// <summary>
        /// Gets the minimum timespan between GET- and POST-request.
        /// </summary>
        public int Timespan { get; private set; }

        /// <summary>
        /// Called when a process requests authorization.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            long timestamp = long.MaxValue;
            var request = filterContext.RequestContext.HttpContext.Request;

            if (long.TryParse(request.Form["SpamProtectionTimeStamp"], out timestamp))
            {
                long currentTime = (long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds;

                if (currentTime <= timestamp + this.Timespan)
                {
                    throw new HttpException(string.Format("Invalid form submission. At least {0} seconds have to pass before form submission ({1}).", this.Timespan, request.Params.ToString()));
                }
            }
            else
            {
                throw new HttpException("Invalid form submission. Invalid timestamp parameter.");
            }

            if (!string.IsNullOrEmpty(request.Form["website"]))
            {
                throw new HttpException(string.Format("Invalid form submission. Invisible field contains value: {0} ({1})", request.Form["website"], request.Params.ToString()));
            }
        }
    }
}
