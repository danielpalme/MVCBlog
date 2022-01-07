namespace MVCBlog.Web.Infrastructure.Excel;

public class QueryableSheet<T> : Sheet<T>
{
    private readonly Func<IEnumerable<T>> getNextElements;

    public QueryableSheet(string name, IEnumerable<Column<T>> columns, Func<IEnumerable<T>> getNextElements)
        : base(name, columns)
    {
        this.getNextElements = getNextElements;
    }

    public override IEnumerable<T> Data
    {
        get
        {
            var elements = this.getNextElements();
            while (elements.Any())
            {
                foreach (var element in elements)
                {
                    yield return element;
                }

                elements = this.getNextElements();
            }
        }
    }
}
