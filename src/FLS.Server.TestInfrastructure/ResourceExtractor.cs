using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLS.Server.TestInfrastructure
{
    public class ResourceExtractor
    {
        private static ResourceExtractor _instance;
        private string ResourceNamespace { get; set; }

        private ResourceExtractor()
        {
            ResourceNamespace = "FLS.Server.TestInfrastructure.Resources.";
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

        //public void ExtractEmailTestResources()
        //{
        //    var asm = Assembly.GetExecutingAssembly();
        //    Assert.IsNotNull(asm);
        //    var resourceNamespace = "FLS.Server.Service.Test.Resources.";
        //    var bin = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        //    var scriptName = "expected-flight-summary-email_de.html";
        //    var insertScript = bin + @"\" + scriptName;
        //    var stream = asm.GetManifestResourceStream(resourceNamespace + scriptName);
        //    Assert.IsNotNull(stream);
        //    var streamReader = new StreamReader(stream);
        //    Assert.IsNotNull(streamReader);
        //    File.WriteAllText(insertScript, streamReader.ReadToEnd());

        //    scriptName = "flight-summary-email_de.tpl";
        //    insertScript = bin + @"\" + scriptName;
        //    stream = asm.GetManifestResourceStream(resourceNamespace + scriptName);
        //    Assert.IsNotNull(stream);
        //    streamReader = new StreamReader(stream);
        //    Assert.IsNotNull(streamReader);
        //    File.WriteAllText(insertScript, streamReader.ReadToEnd());
        //}
    }
}
