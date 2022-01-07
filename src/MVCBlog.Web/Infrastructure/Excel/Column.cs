namespace MVCBlog.Web.Infrastructure.Excel;

public class Column<T>
{
    public Column(string header, Func<T, object> value)
    {
        this.Header = header;
        this.Value = value;
    }

    public string Header { get; set; }

    public Func<T, object> Value { get; set; }
}
