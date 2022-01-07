using Microsoft.AspNetCore.Mvc;

namespace MVCBlog.Web.Controllers;

public class PrivacyController : Controller
{
    public IActionResult Index()
    {
        return this.View();
    }
}