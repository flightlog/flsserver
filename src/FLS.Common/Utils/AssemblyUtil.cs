using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FLS.Common.Utils
{
    public static class AssemblyUtil
    {
        /// <summary>
        /// http://www.programgood.net/CategoryView,category,Silverlight.aspx
        /// </summary>
        public static AssemblyBuildInfo GetAssemblyVersion(string assemblyFullName)
        {
            var assemblyBuildInfo = new AssemblyBuildInfo();

            var assembly = Assembly.Load(assemblyFullName);

            var assemblyName = new AssemblyName(assembly.FullName);
            
            assemblyBuildInfo.AssemblyName = assemblyName.Name;
            assemblyBuildInfo.AssemblyFullName = assemblyName.FullName;
            assemblyBuildInfo.ManifestModule = assembly.ManifestModule.ToString();
            assemblyBuildInfo.FileVersion =
                assembly.GetCustomAttribute<System.Reflection.AssemblyFileVersionAttribute>().Version;

            Version v = assemblyName.Version;

            if (v == null)
            {
                return null;
            }

            // calculate build date and time too
            int revision = v.Revision;
            var numSecsSinceMidnight = revision * 2;
            TimeSpan t = TimeSpan.FromSeconds(numSecsSinceMidnight);

            // Summer time.
            //t = t.Add(new TimeSpan(0, 1, 0, 0));

            string timeOfBuild = string.Format("{0:D2}:{1:D2}:{2:D2}",
                                               t.Hours,
                                               t.Minutes,
                                               t.Seconds);

            // Date of build
            int build = v.Build;
            DateTime buildDate = new DateTime(2000, 1, 1).Add(new TimeSpan(TimeSpan.TicksPerDay * build));
            string buildDateFormat = buildDate.ToString("dd.MM.yyyy");

            assemblyBuildInfo.Version = v.ToString();
            assemblyBuildInfo.BuildDateTime = buildDate.AddSeconds(numSecsSinceMidnight);


            var message = v + "   Build Date: " + buildDateFormat + " " + timeOfBuild;

             return assemblyBuildInfo;
        }
    }
}
