using System;
using System.Linq;
using FLS.Data.WebApi;
using FLS.Data.WebApi.Person;
using FLS.Server.Service;
using FLS.Server.Service.Identity;
using FLS.Server.TestInfrastructure;
using FLS.Server.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using NLog;
using System.IO;

namespace FLS.Server.Tests.ServiceTests
{
    [TestClass]
    public class PersonServiceTest : BaseTest
    {
        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void PersonServiceTestInitialize(TestContext testContext)
        {
            //Stopwatch sw = Stopwatch.StartNew();
            //using (var context = new FLSDataEntities())
            //{
            //    var countries = context.Countries.ToList();
            //}
            //sw.Stop();
            //Console.WriteLine(string.Format("Database connection and loading all countries took: {0} ms", sw.ElapsedMilliseconds));
        }
        
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        [TestMethod]
        [TestCategory("Service")]
        public void ImportPersonExcelFileTest()
        {
            var fullfilename = @"C:\Projects\flsserver\Data\FGZOAdressexportFLSformatted.xlsx";
            var bytes = File.ReadAllBytes(fullfilename);
            PersonService.ImportPersonExcelFile(bytes);
        }
        
    }
}
