using System;
using System.Linq;
using FLS.Data.WebApi.Person;
using FLS.Server.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

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
        public void UpdatePersonPersonCategoryTest()
        {
            
            SetCurrentUser(TestConfigurationSettings.Instance.TestClubAdminUsername);

            var personDetails = new PersonDetails()
            {
                Lastname = "Test",
                Firstname = DateTime.Now.Ticks.ToString(),
                ZipCode = "8000",
                ClubRelatedPersonDetails = new ClubRelatedPersonDetails()
                {
                    IsActive = true,
                    MemberNumber = "9999999"
                }
            };

            PersonService.InsertPersonDetails(personDetails);

            Assert.IsNotNull(personDetails);
            Assert.AreNotEqual(Guid.Empty, personDetails.PersonId);

            var personCategories = ClubService.GetPersonCategories();
            var personCategoryId = personCategories.First().PersonCategoryId;
            personDetails.ClubRelatedPersonDetails.PersonCategoryIds.Add(personCategoryId);

            PersonService.UpdatePersonDetails(personDetails);

            Assert.IsTrue(personDetails.ClubRelatedPersonDetails.PersonCategoryIds.Contains(personCategoryId));
        }

        [TestMethod]
        [TestCategory("Service")]
        public void InsertPersonPersonCategoryTest()
        {

            SetCurrentUser(TestConfigurationSettings.Instance.TestClubAdminUsername);

            var personDetails = new PersonDetails()
            {
                Lastname = "Test",
                Firstname = DateTime.Now.Ticks.ToString(),
                ZipCode = "8000",
                ClubRelatedPersonDetails = new ClubRelatedPersonDetails()
                {
                    IsActive = true,
                    MemberNumber = "9999999",
                    PersonCategoryIds = new List<Guid>()
                }
            };

            var personCategories = ClubService.GetPersonCategories();
            var personCategoryId = personCategories.First().PersonCategoryId;
            personDetails.ClubRelatedPersonDetails.PersonCategoryIds.Add(personCategoryId);

            PersonService.InsertPersonDetails(personDetails);

            Assert.IsNotNull(personDetails);
            Assert.AreNotEqual(Guid.Empty, personDetails.PersonId);
            Assert.IsTrue(personDetails.ClubRelatedPersonDetails.PersonCategoryIds.Contains(personCategoryId));
        }
    }
}
