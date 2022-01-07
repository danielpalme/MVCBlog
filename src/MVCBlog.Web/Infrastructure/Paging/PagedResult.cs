using System.Collections;
using System.Runtime.Serialization;

namespace MVCBlog.Web.Infrastructure.Paging;

/// <summary>
/// Wrappes a list of items together with the total number of available items.
/// </summary>
/// <typeparam name="T">The type of the items.</typeparam>
[DataContract]
public class PagedResult<T> : IEnumerable<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PagedResult&lt;T&gt;"/> class.
    /// </summary>
    public PagedResult()
    {
        this.Items = new List<T>();
        this.Paging = new Paging<T>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PagedResult&lt;T&gt;"/> class.
    /// </summary>
    /// <param name="items">The items.</param>
    /// <param name="totalNumberOfItems">The total number of items.</param>
    /// <param name="paging">The paging.</param>
    public PagedResult(IEnumerable<T> items, int totalNumberOfItems, Paging<T> paging)
    {
        this.Items = items;
        this.TotalNumberOfItems = totalNumberOfItems;
        this.Paging = paging;
    }

    /// <summary>
    /// Gets the items.
    /// </summary>
    [DataMember]
    public IEnumerable<T> Items { get; private set; }

    public Paging<T> Paging { get; private set; }

    /// <summary>
    /// Gets the total number of items.
    /// </summary>
    [DataMember]
    public int TotalNumberOfItems { get; private set; }

    public IEnumerator<T> GetEnumerator()
    {
        return this.Items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.Items.GetEnumerator();
    }
}