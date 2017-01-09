using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLS.Data.WebApi
{
    public class PagedList<T>
    {
        private readonly List<T> _items;
        private readonly int _pageStart;
        private readonly int _pageSize;
        private readonly int _totalRows;

        public PagedList(List<T> items, int pageStart, int pageSize, int totalRows)
        {
            _items = items;
            _pageStart = pageStart;
            _pageSize = pageSize;
            _totalRows = totalRows;
        }

        public List<T> Items { get { return _items; } }

        public int PageStart { get { return _pageStart; } }

        public int PageSize { get { return _pageSize; } }

        public int TotalRows { get { return _totalRows; } }
    }
}
