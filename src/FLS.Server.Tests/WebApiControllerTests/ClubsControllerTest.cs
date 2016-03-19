using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FLS.Common.Extensions;
using FLS.Data.WebApi.Club;
using FLS.Data.WebApi.User;
using FLS.Server.TestInfrastructure;
using FLS.Server.Tests.Helpers;
using FLS.Server.Tests.Infrastructure.WebApi;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLS.Server.Tests.WebApiControllerTests
{
    [TestClass]
    public class ClubsControllerTest : BaseAuthenticatedTests
    {
        [TestMethod]
        [TestCategory("WebApi")]
        public void GetClubOverviewsWebApiTest()
        {
            var response = GetAsync<IEnumerable<ClubOverview>>(Uri + "/overview").Result;
            Assert.IsTrue(response.Any());
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void GetMyClubDetailsWebApiTest()
        {
            var clubDetails = GetAsync<ClubDetails>(Uri + "/my").Result;
            Assert.IsNotNull(clubDetails);

            var userDetails = GetAsync<UserDetails>("api/v1/users/my").Result;
            Assert.IsNotNull(userDetails);

            Assert.AreEqual(clubDetails.ClubId, userDetails.ClubId, "ClubId is not equals between user and club (my)");
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void GetClubDetailsWebApiTest()
        {
            var response = GetAsync<IEnumerable<ClubOverview>>(Uri + "/overview").Result;
            Assert.IsTrue(response.Any());

            foreach (var clubOverview in response)
            {
                var clubDetails = GetAsync<ClubDetails>(Uri + "/" + clubOverview.ClubId).Result;
                Assert.IsNotNull(clubDetails);
            }
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void InsertClubDetailsWebApiTest()
        {
            LoginAsSystemAdmin();
            var clubDetails = InsertClubDetailsWebApi();
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void UpdateClubDetailsWebApiTest()
        {
            var clubDetails = GetAsync<ClubDetails>(Uri + "/my").Result;
            Assert.IsNotNull(clubDetails);

            if (clubDetails.DefaultStartType.HasValue)
            {
                clubDetails.DefaultStartType = GetStartTypes().First(a => a.StartTypeId != clubDetails.DefaultStartType).StartTypeId;
            }
            else
            {
                clubDetails.DefaultStartType = GetStartTypes().First().StartTypeId;
            }

            clubDetails.ContactName = DateTime.Now.ToShortTimeString();

            var putResult = PutAsync(clubDetails,  Uri  + "/" + clubDetails.ClubId).Result;

            Assert.IsTrue(putResult.IsSuccessStatusCode);
        }

        [TestMethod()]
        [TestCategory("WebApi")]
        public void DeleteClubWebApiTest()
        {
            LoginAsSystemAdmin();

            //insert new club first, which will be deleted afterwards
            var clubDetails = InsertClubDetailsWebApi();

            //load clubs again to see if club is really created
            var clubs = GetAsync<IEnumerable<ClubOverview>>(Uri).Result;
            Assert.IsNotNull(clubs);
            Assert.IsTrue(clubs.Any());
            var club = clubs.FirstOrDefault(c => c.ClubId == clubDetails.ClubId);

            Assert.IsNotNull(club);

            var delResult = DeleteAsync(Uri + "/" + club.ClubId).Result;

            Assert.IsTrue(delResult.IsSuccessStatusCode);

            var clubsNew = GetAsync<IEnumerable<ClubOverview>>(Uri).Result;

            Assert.IsTrue(clubsNew.Count() < clubs.Count());
        }

        [TestMethod()]
        [TestCategory("WebApi")]
        public void DeleteClubWithActiveUsersWebApiTest()
        {
            LoginAsSystemAdmin();

            //insert new club first, which will be deleted afterwards
            var clubDetails = InsertClubDetailsWebApi();

            //Create club user
            SetCurrentUser(TestConfigurationSettings.Instance.TestSystemAdminUsername);
            var user = CreateNewUserInDb(clubDetails.ClubId, "blabla");

            //load clubs again to see if club is really created
            var clubs = GetAsync<IEnumerable<ClubOverview>>(Uri).Result;
            Assert.IsNotNull(clubs);
            Assert.IsTrue(clubs.Any());
            var club = clubs.FirstOrDefault(c => c.ClubId == clubDetails.ClubId);

            Assert.IsNotNull(club);

            var delResult = DeleteAsync(Uri + "/" + club.ClubId).Result;

            Assert.AreEqual(delResult.StatusCode, HttpStatusCode.BadRequest, "Club deletion must result in error, as active users are available!");

            var clubsNew = GetAsync<IEnumerable<ClubOverview>>(Uri).Result;

            Assert.AreEqual(clubsNew.Count(), clubs.Count());
        }

        protected override string Uri
        {
            get { return RoutePrefix; }
        }

        protected override string RoutePrefix
        {
            get { return "/api/v1/clubs"; }
        }

        #region Private methods
        private ClubDetails InsertClubDetailsWebApi()
        {
            var clubDetails = CreateClubDetails();

            var response = PostAsync(clubDetails, Uri).Result;

            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            var responseDetails = ConvertToModel<ClubDetails>(response);
            Assert.IsNotNull(responseDetails);
            Assert.IsTrue(responseDetails.Id.IsValid(), string.Format("Primary key not set/mapped after insert or update. Entity-Info: {0}", responseDetails));
            Assert.AreEqual(clubDetails.HomebaseId, responseDetails.HomebaseId);
            Assert.AreEqual(clubDetails.DefaultStartType, responseDetails.DefaultStartType);
            Assert.AreEqual(clubDetails.DefaultGliderFlightTypeId, responseDetails.DefaultGliderFlightTypeId);
            Assert.AreEqual(clubDetails.DefaultMotorFlightTypeId, responseDetails.DefaultMotorFlightTypeId);
            Assert.AreEqual(clubDetails.DefaultTowFlightTypeId, responseDetails.DefaultTowFlightTypeId);

            return responseDetails;
        }
        #endregion Private methods
    }
}
