using System;
using System.Collections.Generic;
using System.Linq;
using FLS.Common.Extensions;
using FLS.Data.WebApi.Club;
using FLS.Server.Tests.Infrastructure.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLS.Server.Tests.WebApiControllerTests
{
    [TestClass]
    public class PersonCategoriesControllerTest : BaseAuthenticatedTests
    {
        [TestMethod]
        [TestCategory("WebApi")]
        public void GetPersonCategoryOverviewWebApiTest()
        {
            InsertPersonCategoryDetailsWebApiTest();

            var response = GetAsync<IEnumerable<PersonCategoryOverview>>("/api/v1/personcategories").Result;

            Assert.IsTrue(response.Any());
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void GetPersonCategoryDetailsWebApiTest()
        {
            InsertPersonCategoryDetailsWebApiTest();

            var response = GetAsync<IEnumerable<PersonCategoryOverview>>("/api/v1/personcategories").Result;

            Assert.IsTrue(response.Any());

            var id = response.First().PersonCategoryId;

            var result = GetAsync<PersonCategoryDetails>("/api/v1/personcategories/" + id).Result;

            Assert.AreEqual(id, result.Id);
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void InsertPersonCategoryDetailsWebApiTest()
        {
            var category = new PersonCategoryDetails();
            category.CategoryName = "PersonCategory @ " + DateTime.Now.Ticks;
            category.Remarks = "Test";

            var response = PostAsync(category, "/api/v1/personcategories").Result;

            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            var responseDetails = ConvertToModel<PersonCategoryDetails>(response);
            Assert.IsTrue(responseDetails.Id.IsValid(), string.Format("Primary key not set/mapped after insert or update. Entity-Info: {0}", responseDetails));
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void InsertPersonCategoryHierarchyDetailsWebApiTest()
        {
            var activeMemberCategory = new PersonCategoryDetails();
            activeMemberCategory.CategoryName = "Active member";
            activeMemberCategory.Remarks = "active members";

            var response = PostAsync(activeMemberCategory, "/api/v1/personcategories").Result;

            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            var rootDetails = ConvertToModel<PersonCategoryDetails>(response);
            Assert.IsTrue(rootDetails.Id.IsValid(), string.Format("Primary key not set/mapped after insert or update. Entity-Info: {0}", rootDetails));

            var category = new PersonCategoryDetails();
            category.CategoryName = "Gliderpilots";
            category.ParentPersonCategoryId = rootDetails.PersonCategoryId;

            response = PostAsync(category, "/api/v1/personcategories").Result;

            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            var responseDetails = ConvertToModel<PersonCategoryDetails>(response);
            Assert.IsTrue(responseDetails.Id.IsValid(), string.Format("Primary key not set/mapped after insert or update. Entity-Info: {0}", responseDetails));

            var category2 = new PersonCategoryDetails();
            category2.CategoryName = "Motorpilots";
            category2.ParentPersonCategoryId = rootDetails.PersonCategoryId;

            response = PostAsync(category2, "/api/v1/personcategories").Result;

            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            responseDetails = ConvertToModel<PersonCategoryDetails>(response);
            Assert.IsTrue(responseDetails.Id.IsValid(), string.Format("Primary key not set/mapped after insert or update. Entity-Info: {0}", responseDetails));

            var passant = new PersonCategoryDetails();
            passant.CategoryName = "Passant";

            response = PostAsync(passant, "/api/v1/personcategories").Result;

            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            responseDetails = ConvertToModel<PersonCategoryDetails>(response);
            Assert.IsTrue(responseDetails.Id.IsValid(), string.Format("Primary key not set/mapped after insert or update. Entity-Info: {0}", responseDetails));
        }
        
        [TestMethod]
        [TestCategory("WebApi")]
        public void UpdatePersonCategoryDetailsWebApiTest()
        {
            InsertPersonCategoryDetailsWebApiTest();

            var response = GetAsync<IEnumerable<PersonCategoryOverview>>("/api/v1/personcategories").Result;

            Assert.IsTrue(response.Any());

            var id = response.First().PersonCategoryId;

            var result = GetAsync<PersonCategoryDetails>("/api/v1/personcategories/" + id).Result;

            Assert.AreEqual(id, result.Id);

            result.Remarks = "Updated on " + DateTime.Now.ToShortTimeString();

            var putResult = PutAsync(result, "/api/v1/personcategories/" + id).Result;

            Assert.IsTrue(putResult.IsSuccessStatusCode);
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void DeletePersonCategoryDetailsWebApiTest()
        {
            InsertPersonCategoryDetailsWebApiTest();

            var response = GetAsync<IEnumerable<PersonCategoryOverview>>("/api/v1/personcategories").Result;

            Assert.IsTrue(response.Any());

            var id = response.First().PersonCategoryId;

            var delResult = DeleteAsync("/api/v1/personcategories/" + id).Result;

            Assert.IsTrue(delResult.IsSuccessStatusCode);
        }

        protected override string Uri
        {
            get { return RoutePrefix; }
        }

        protected override string RoutePrefix
        {
            get { return "/api/v1/personcategories"; }
        }
    }
}
