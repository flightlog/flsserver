using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLS.Data.WebApi.DataExchange
{
    public class ImportObject<T>
    {
        public T ImportDataRecord { get; set; }

        public T ServerDataRecord { get; set; }

        public ImportState ImportState { get; set; }

        public string ErrorMessage { get; set; }

        public bool HasError { get; set; }
    }
}
