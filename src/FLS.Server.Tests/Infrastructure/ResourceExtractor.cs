using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLS.Server.Tests.Infrastructure
{
    public class ResourceExtractor
    {
        private static ResourceExtractor _instance;
        private string ResourceNamespace { get; set; }

        private ResourceExtractor()
        {
            ResourceNamespace = "FLS.Server.Tests.Resources.";
        }

        public static ResourceExtractor Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ResourceExtractor();
                }

                return _instance;
            }
        }

        public string ExtractSqlScript(string scriptName)
        {
            var asm = Assembly.GetExecutingAssembly();
            Assert.IsNotNull(asm);
            var stream = asm.GetManifestResourceStream(ResourceNamespace + scriptName);
            Assert.IsNotNull(stream);
            var sql = string.Empty;

            using (var streamReader = new StreamReader(stream))
            {
                Assert.IsNotNull(streamReader);
                sql = streamReader.ReadToEnd();
            }

            sql = RemoveConflictingSqlCommandParts(sql);
            return sql;
        }

        private string RemoveConflictingSqlCommandParts(string sql)
        {
            Regex rx = new Regex(@"USE \[(\w*)\](.*?)GO", RegexOptions.Singleline);
            var output = rx.Replace(sql, "");

            rx = new Regex(@"^GO", RegexOptions.Multiline);
            output = rx.Replace(sql, "");
            return output;
        }
    }
}
