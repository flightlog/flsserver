using FLS.Common.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLS.Server.Tests.WebApiControllerTests
{
    [TestClass]
    public class AssemblyUtilTest
    {
        [TestMethod]
        [TestCategory("FLS.Common")]
        public void GetAssemblyInfo()
        {
            var assemblyVersion = AssemblyUtil.GetAssemblyVersion("FLS.Common");
            Assert.IsNotNull(assemblyVersion);

            assemblyVersion = AssemblyUtil.GetAssemblyVersion("FLS.Server.WebApi");
            Assert.IsNotNull(assemblyVersion);
        }
    }
}
