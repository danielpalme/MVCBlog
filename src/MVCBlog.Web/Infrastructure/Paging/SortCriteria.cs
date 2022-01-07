using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace MVCBlog.Web.Infrastructure.Paging;

[DataContract]
public class SortCriteria<T>
{
    public SortCriteria(string sortColumn)
    {
        this.SortDirection = SortDirection.Ascending;
        this.SortColumn = sortColumn;
    }

    public SortCriteria(Expression<Func<T, object>> expression)
    {
        this.SortDirection = SortDirection.Ascending;
        this.SortColumn = PropertyResolver.GetPropertyName(expression);
    }

    public SortCriteria(string sortColumn, SortDirection sortDirection)
    {
        this.SortDirection = sortDirection;
        this.SortColumn = sortColumn;
    }

    public SortCriteria(Expression<Func<T, object>> expression, SortDirection sortDirection)
    {
        this.SortDirection = sortDirection;
        this.SortColumn = PropertyResolver.GetPropertyName(expression);
    }

    /// <summary>
    /// Gets or sets the <see cref="SortDirection"/>.
    /// </summary>
    [DataMember]
    public SortDirection SortDirection { get; set; }

    /// <summary>
    /// Gets or sets the name of the property sorting should be applied to.
    /// </summary>
    [DataMember]
    public string SortColumn { get; set; }

    /// <summary>
    /// Sets the sort expression.
    /// </summary>
    /// <param name="expression">A lambda expression like 'n => n.PropertyName'.</param>
    public void SetSortExpression(Expression<Func<T, object>> expression)
    {
        this.SortColumn = PropertyResolver.GetPropertyName(expression);
    }

    /// <summary>
    /// Returns a <see cref="string"/> that represents this instance.
    /// </summary>
    /// <returns>
    /// A <see cref="string"/> that represents this instance.
    /// </returns>
    public override string ToString()
    {
        return $"SortColumn: {this.SortColumn}";
    }
}