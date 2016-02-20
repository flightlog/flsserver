using System;

namespace FLS.Data.WebApi.Audit
{
    public class PropertyChangeLogDetails
    {
        public string PropertyName { get; set; }

        public string OriginalValue { get; set; }

        public string NewValue { get; set; }
    }
}
