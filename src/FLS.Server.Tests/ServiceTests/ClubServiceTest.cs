using System;
using System.Linq;
using FLS.Data.WebApi;
using FLS.Data.WebApi.Club;
using FLS.Server.Service;
using FLS.Server.Service.Identity;
using FLS.Server.TestInfrastructure;
using FLS.Server.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using NLog;

namespace FLS.Server.Tests.ServiceTests
{
    [TestClass]
    public class ClubServiceTest : BaseTest
    {
        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void ClubServiceTestInitialize(TestContext testContext)
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
        public void HierarchicalPersonCategoryTest()
        {
            var personCategories = ClubService.GetPersonCategoryOverviews();

            var rootCategory = new PersonCategoryDetails()
            {
                CategoryName = "Root"
            };

            ClubService.InsertPersonCategoryDetails(rootCategory);

            var gliderCategory = new PersonCategoryDetails()
            {
                CategoryName = "GliderPilots",
                ParentPersonCategoryId = rootCategory.PersonCategoryId
            };

            ClubService.InsertPersonCategoryDetails(gliderCategory);

            var gliderTraineeCategory = new PersonCategoryDetails()
            {
                CategoryName = "GliderTrainees",
                ParentPersonCategoryId = gliderCategory.PersonCategoryId
            };

            ClubService.InsertPersonCategoryDetails(gliderTraineeCategory);

            var gliderInstructorsCategory = new PersonCategoryDetails()
            {
                CategoryName = "GliderInstructors",
                ParentPersonCategoryId = gliderCategory.PersonCategoryId
            };

            ClubService.InsertPersonCategoryDetails(gliderInstructorsCategory);

            var motorCategory = new PersonCategoryDetails()
            {
                CategoryName = "Motorpilots",
                ParentPersonCategoryId = rootCategory.PersonCategoryId
            };

            ClubService.InsertPersonCategoryDetails(motorCategory);

            //test the records

            var categories = ClubService.GetPersonCategoryOverviews();
            categories.ForEach(x => Logger.Debug($"{x.ToString()}"));

            var filter = new PageableSearchFilter<PersonCategoryOverviewSearchFilter>();
            filter.SearchFilter = new PersonCategoryOverviewSearchFilter();
            filter.SearchFilter.CategoryName = gliderCategory.CategoryName;
            var pagedCategories = ClubService.GetPagedPersonCategoryOverview(1, 1, filter);
            pagedCategories.Items.ForEach(x => Logger.Debug($"{x.ToString()}"));

            //TODO: Implement and test loading of parents when filter records (bool loadParents)
        }
        
    }
}
