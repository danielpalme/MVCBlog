using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVCBlog.Website
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            // Short URL for '404'
            routes.MapRoute(
                "Short_404",
                "404",
                new { controller = MVC.Error.Name, action = MVC.Error.ActionNames.NotFound });

            // Short URL for 'Logout'
            routes.MapRoute(
                "Short_Logout",
                "Logout",
                new { controller = MVC.Login.Name, action = MVC.Login.ActionNames.Logout });

            routes.MapRoute(
                Routes.BLOGENTRY,
                MVC.Blog.Name + "/{year}/{month}/{day}/{id}",
                new { controller = MVC.Blog.Name, action = MVC.Blog.ActionNames.Entry },
                new { year = "\\d{4}", month = "\\d{1,2}", day = "\\d{1,2}", id = ".+" });

            routes.MapRoute(
                Routes.TAGPAGING,
               MVC.Blog.Name + "/Tag/{tag}/Page/{page}",
               new { controller = MVC.Blog.Name, action = MVC.Blog.ActionNames.Index },
               new { tag = ".+", page = "\\d+" });

            routes.MapRoute(
               Routes.TAG,
               MVC.Blog.Name + "/Tag/{tag}",
               new { controller = MVC.Blog.Name, action = MVC.Blog.ActionNames.Index },
               new { tag = ".+" });

            routes.MapRoute(
               Routes.BLOGPAGING,
               MVC.Blog.Name + "/Page/{page}",
               new { controller = MVC.Blog.Name, action = MVC.Blog.ActionNames.Index },
               new { page = "\\d+" });

            routes.MapRoute(
                Routes.DEFAULT,
                "{controller}/{action}/{id}",
                new { controller = MVC.Blog.Name, action = MVC.Blog.ActionNames.Index, id = UrlParameter.Optional });
        }
    }
}
