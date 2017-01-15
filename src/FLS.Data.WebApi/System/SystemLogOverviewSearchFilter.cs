using System;

namespace FLS.Data.WebApi.System
{
    public class SystemLogOverviewSearchFilter
    {
        public string EventDateTime { get; set; }

        public string LogLevel { get; set; }

        public string EventType { get; set; }

        public string Logger { get; set; }

        public string Message { get; set; }

        public string UserName { get; set; }
    }
}
