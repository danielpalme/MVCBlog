using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVCBlog.Web.Models;

namespace MVCBlog.Web.Controllers;

public class ErrorController : Controller
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Index(int? code)
    {
        return this.View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
    }

    [Route("Error/404")]
    public IActionResult Error404()
    {
        return this.View();
    }
}