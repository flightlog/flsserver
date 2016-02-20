using System;
using System.Collections.Generic;

namespace FLS.Data.WebApi.System
{
    public class SystemVersionInfoOverview
    {
        public string DatabaseSchemaVersion { get; set; }

        public string Version { get; set; }

        public DateTime BuildDateTime { get; set; }
    }
}
