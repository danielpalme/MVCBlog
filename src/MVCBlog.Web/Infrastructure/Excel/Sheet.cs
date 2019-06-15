using System;
using System.Collections.Generic;
using System.Linq;

namespace MVCBlog.Web.Infrastructure.Excel
{
    public class Sheet<T>
    {
        public Sheet(string name, IEnumerable<Column<T>> columns, IEnumerable<T> data)
        {
            this.Name = name;
            this.Columns = columns;
            this.Data = data;
        }

        protected Sheet()
        {
        }

        public string Name { get; protected set; }

        public IEnumerable<Column<T>> Columns { get; protected set; }

        public virtual IEnumerable<T> Data { get; private set; }
    }

    public class QueryableSheet<T> : Sheet<T>
    {
        private readonly Func<IEnumerable<T>> getNextElements;

        public QueryableSheet(string name, IEnumerable<Column<T>> columns, Func<IEnumerable<T>> getNextElements)
        {
            this.Name = name;
            this.Columns = columns;
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
}
