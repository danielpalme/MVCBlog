using System.Runtime.Serialization;
using System.Web.UI.WebControls;

namespace Palmmedia.Common.Linq
{
    /// <summary>
    /// Contains information necessary for paging and sorting.
    /// <seealso cref="PagingExtensions"/>.
    /// </summary>
    [DataContract]
    public class Paging
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Paging"/> class.
        /// </summary>
        public Paging()
            : this(1, 10)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Paging" /> class.
        /// </summary>
        /// <param name="page">The current page.</param>
        /// <param name="pageSize">Number of elements per page.</param>
        public Paging(int page, int pageSize)
        {
            this.SortDirection = SortDirection.Ascending;
            this.PageIndex = page > 0 ? page - 1 : 0;
            this.PageSize = pageSize;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Paging" /> class.
        /// </summary>
        /// <param name="page">The current page.</param>
        /// <param name="pageSize">Number of elements per page.</param>
        /// <param name="sort">The name of the property sorting should be applied to.</param>
        public Paging(int page, int pageSize, string sort)
            : this(page, pageSize)
        {
            this.SortColumn = sort;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Paging" /> class.
        /// </summary>
        /// <param name="page">The current page.</param>
        /// <param name="pageSize">Number of elements per page.</param>
        /// <param name="sort">The name of the property sorting should be applied to.</param>
        /// <param name="sortDir">The <see cref="SortDirection"/>.</param>
        public Paging(int page, int pageSize, string sort, SortDirection sortDir)
            : this(page, pageSize, sort)
        {
            this.SortDirection = sortDir;
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
        /// Gets or sets the current page index.
        /// </summary>
        [DataMember]
        public int PageIndex { get; set; }

        /// <summary>
        /// Gets or sets the number of elements per page.
        /// </summary>
        [DataMember]
        public int PageSize { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "SortColumn: " + this.SortColumn + ", PageIndex: " + this.PageIndex + ", PageSize: " + this.PageSize;
        }
    }
}
