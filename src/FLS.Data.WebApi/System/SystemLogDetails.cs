using System;

namespace FLS.Data.WebApi.System
{
    public class SystemLogDetails
    {
        public long LogId { get; set; }

        public DateTime EventDateTime { get; set; }

        public string Application { get; set; }

        public string LogLevel { get; set; }

        public long? EventType { get; set; }

        public string Logger { get; set; }

        public string Message { get; set; }

        public string UserName { get; set; }

        public string ComputerName { get; set; }

        public string CallSite { get; set; }

        public string Thread { get; set; }

        public string Exception { get; set; }

        public string Stacktrace { get; set; }
    }
}
