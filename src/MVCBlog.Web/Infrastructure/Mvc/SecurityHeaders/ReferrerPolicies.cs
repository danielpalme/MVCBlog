namespace MVCBlog.Web.Infrastructure.Mvc.SecurityHeaders;

public enum ReferrerPolicies
{
    None,

    Empty,

    NoReferrer,

    NoReferrerWhenDowngrade,

    SameOrigin,

    Origin,

    StrictOrigin,

    OriginWhenCrossOrigin,

    StrictOriginWhenCrossOrigin,

    UnsafeUrl
}
