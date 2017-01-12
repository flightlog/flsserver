using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLS.Data.WebApi
{
    public class PageableSearchFilter<T>
    {
        public PageableSearchFilter()
        {
            Sorting = new Dictionary<string, string>();
        }
        public Dictionary<string, string> Sorting { get; set; }

        public T SearchFilter { get; set; }
    }
}
