using System;
using System.Collections.Generic;
using System.Web.WebPages;

namespace Palmmedia.Common.Net.Mvc
{
    /// <summary>
    /// Helper method to render a template for each item in a collection with the possibility to access the
    /// index of the current item.
    /// See: http://haacked.com/archive/2011/04/14/a-better-razor-foreach-loop.aspx
    /// </summary>
    public static class RazorForeachLoop
    {
        /// <summary>
        /// Renders the given template for each item.
        /// It is possible to use the index of the current item.
        /// </summary>
        /// <typeparam name="T">The type of the model.</typeparam>
        /// <param name="items">The items.</param>
        /// <param name="template">The template.</param>
        /// <example>
        /// @Model.Each(@<li>Item @item.Index of @(Model.Count()): @item.Item.Title</li>)
        /// </example>
        /// <returns>The <see cref="HelperResult"/> containing the rendered template for each item.</returns>
        public static HelperResult Each<T>(this IEnumerable<T> items, Func<IndexedItem<T>, HelperResult> template)
        {
            return new HelperResult(writer =>
            {
                int index = 0;

                foreach (var item in items)
                {
                    var result = template(new IndexedItem<T>(index++, item));
                    result.WriteTo(writer);
                }
            });
        }

        /// <summary>
        /// Helper class which is used to index item in <see cref="RazorForeachLoop"/>.
        /// </summary>
        /// <typeparam name="T">The type of the model.</typeparam>
        public class IndexedItem<T>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="IndexedItem&lt;T&gt;"/> class.
            /// </summary>
            /// <param name="index">The index.</param>
            /// <param name="item">The item.</param>
            public IndexedItem(int index, T item)
            {
                this.Index = index;
                this.Item = item;
            }

            /// <summary>
            /// Gets the index.
            /// </summary>
            public int Index { get; private set; }

            /// <summary>
            /// Gets the item.
            /// </summary>
            public T Item { get; private set; }
        }
    }
}
