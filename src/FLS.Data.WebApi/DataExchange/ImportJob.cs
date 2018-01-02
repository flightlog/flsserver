using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLS.Data.WebApi.DataExchange
{
    public class ImportJob<T>
    {
        public List<ImportObject<T>> ImportObjects { get; set; }

        public SortedDictionary<int, List<object>> AdditionalEntitiesToCreate { get; set; }

        public bool HasErrors { get; set; }
    }
}
