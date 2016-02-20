using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLS.Common.Utils
{
    public class AssemblyBuildInfo
    {
        public string AssemblyName { get; set; }

        public string AssemblyFullName { get; set; }

        public string ManifestModule { get; set; }

        public string Version { get; set; }

        public string FileVersion { get; set; }

        public DateTime BuildDateTime { get; set; }
    }
}
