using System;
using System.Collections.Generic;

namespace FLS.Data.WebApi.Audit
{
    public class AuditLogOverview
    {
        public AuditLogOverview()
        {
            PropertyChanges = new List<PropertyChangeLogDetails>();
        }

        public long AuditLogId { get; set; }

        public DateTime EventDateTime { get; set; }

        public string UserName { get; set; }

        public string EventTypeName { get; set; }

        public string EntityName { get; set; }

        public string RecordId { get; set; }

        public List<PropertyChangeLogDetails> PropertyChanges { get; set; }
    }
}
