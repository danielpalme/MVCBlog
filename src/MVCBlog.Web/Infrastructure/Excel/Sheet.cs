namespace MVCBlog.Web.Infrastructure.Excel;

public class Sheet<T>
{
    public Sheet(string name, IEnumerable<Column<T>> columns, IEnumerable<T> data)
        : this(name, columns)
    {
        this.Data = data;
    }

    protected Sheet(string name, IEnumerable<Column<T>> columns)
    {
        this.Name = name;
        this.Columns = columns;
    }

    public string Name { get; protected set; }

    public IEnumerable<Column<T>> Columns { get; protected set; }

    public virtual IEnumerable<T> Data { get; } = Enumerable.Empty<T>();
}