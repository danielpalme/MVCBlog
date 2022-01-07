using MVCBlog.Data;
using MVCBlog.Web.Infrastructure.Paging;

namespace MVCBlog.Web.Models.Administration;

public class UsersViewModel
{
    public string? SearchTerm { get; set; }

    public PagedResult<User>? Users { get; set; }
}
