using System;

namespace MVCBlog.Web.Infrastructure.Excel
{
    public class Column<T>
    {
        public string Header { get; set; }

        public Func<T, object> Value { get; set; }
    }
}
