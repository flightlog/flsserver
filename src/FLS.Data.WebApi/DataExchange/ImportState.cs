using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLS.Data.WebApi.DataExchange
{
    public enum ImportState
    {
        Unknown = 0,
        ImportedSuccessfully = 1,
        UpdatedSuccessfully = 2,
        ImportError = 3,
        UpdateError = 4,
        ValidationError = 7,
        Duplicate = 8,
        Ignored = 9
    }
}
