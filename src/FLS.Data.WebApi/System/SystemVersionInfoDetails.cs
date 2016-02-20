using System.Collections.Generic;

namespace FLS.Data.WebApi.System
{
    public class SystemVersionInfoDetails
    {
        public SystemVersionInfoDetails()
        {
            AssembliesInfo = new List<AssemblyInfo>();
        }

        public string DatabaseSchemaVersion { get; set; }

        public List<AssemblyInfo> AssembliesInfo { get; set; }
    }
}
