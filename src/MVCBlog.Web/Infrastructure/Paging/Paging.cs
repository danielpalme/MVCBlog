using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace MVCBlog.Web.Infrastructure.Paging;

/// <summary>
/// Contains information necessary for paging and sorting.
/// <seealso cref="PagingExtensions" />.
/// </summary>
/// <typeparam name="T">The type.</typeparam>
[DataContract]
public class Paging<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Paging{T}"/> class.
    /// </summary>
    public Paging()
        : this(0, 20)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Paging{T}"/> class.
    /// </summary>
    /// <param name="skip">The number of elements to skip.</param>
    /// <param name="top">The number of elements to retrieve.</param>
    public Paging(int skip, int top)
    {
        this.SortDirection = SortDirection.Ascending;
        this.Skip = skip;
        this.Top = top;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Paging{T}"/> class.
    /// </summary>
    /// <param name="skip">The number of elements to skip.</param>
    /// <param name="top">The number of elements to retrieve.</param>
    /// <param name="sortColumn">The name of the property sorting should be applied to.</param>
    public Paging(int skip, int top, string sortColumn)
        : this(skip, top)
    {
        this.SortColumn = sortColumn;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Paging{T}"/> class.
    /// </summary>
    /// <param name="skip">The number of elements to skip.</param>
    /// <param name="top">The number of elements to retrieve.</param>
    /// <param name="sortColumn">The name of the property sorting should be applied to.</param>
    /// <param name="sortDirection">The <see cref="SortDirection"/>.</param>
    public Paging(int skip, int top, string sortColumn, SortDirection sortDirection)
        : this(skip, top, sortColumn)
    {
        this.SortDirection = sortDirection;
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
    public string? SortColumn { get; set; }

    [DataMember]
    public List<SortCriteria<T>> AdditionalSortCriteria { get; } = new List<SortCriteria<T>>();

    /// <summary>
    /// Gets or sets the number of elements to skip.
    /// </summary>
    [DataMember]
    public int Skip { get; set; }

    /// <summary>
    /// Gets or sets the number of elements to retrieve.
    /// </summary>
    [DataMember]
    public int Top { get; set; }

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
        return $"SortColumn: {this.SortColumn}, Top: {this.Top}, Skip: {this.Skip}";
    }
}