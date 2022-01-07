using System;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MVCBlog.Web.Infrastructure.Mvc;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class SpamProtectionAttribute : ActionFilterAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SpamProtectionAttribute"/> class.
    /// </summary>
    public SpamProtectionAttribute()
        : this(4)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SpamProtectionAttribute"/> class.
    /// </summary>
    /// <param name="timespan">The he minimum timespan between GET- and POST-request.</param>
    public SpamProtectionAttribute(int timespan)
    {
        this.Timespan = timespan;
        this.Order = 1000;
    }

    public int Timespan { get; private set; }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        bool spamDetected = false;

        if (long.TryParse(context.HttpContext.Request.Form["SpamProtectionTimeStamp"], out long timestamp))
        {
            if (DateTimeOffset.Now.ToUnixTimeSeconds() <= (timestamp + this.Timespan))
            {
                spamDetected = true;
            }
        }
        else
        {
            // Invalid form submission. Invalid timestamp parameter.
            spamDetected = true;
        }

        if (!string.IsNullOrEmpty(context.HttpContext.Request.Form["Website"]))
        {
            spamDetected = true;
        }

        if (spamDetected)
        {
            context.Result = new RedirectResult(context.HttpContext.Request.GetDisplayUrl());
        }
    }
}
