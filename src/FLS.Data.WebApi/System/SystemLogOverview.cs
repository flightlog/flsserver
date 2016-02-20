using System;

namespace FLS.Data.WebApi.System
{
    public class SystemLogOverview
    {
        public long LogId { get; set; }

        public DateTime EventDateTime { get; set; }

        public string LogLevel { get; set; }

        public long? EventType { get; set; }

        public string Logger { get; set; }

        public string Message { get; set; }

        public string UserName { get; set; }

    }
}
