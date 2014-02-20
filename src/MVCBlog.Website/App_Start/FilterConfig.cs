using System.Configuration;
using System.Web;
using System.Web.Mvc;

namespace MVCBlog.Website
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new Palmmedia.Common.Net.Mvc.HandleErrorAttribute());
            filters.Add(new Palmmedia.Common.Net.Mvc.Localization.LocalizeAttribute());
            filters.Add(new Palmmedia.Common.Net.PingBack.PingbackAttribute(ConfigurationManager.AppSettings["PingbackHandler"]));
        }
    }
}
