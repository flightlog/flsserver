using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLS.Data.WebApi
{
    public class DateTimeFilter
    {
        public DateTime? Fixed { get; set; }

        public DateTime? From { get; set; }

        public DateTime? To { get; set; }
    }
}
