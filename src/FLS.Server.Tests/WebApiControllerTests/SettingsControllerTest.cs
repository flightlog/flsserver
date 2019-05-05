using FLS.Data.WebApi.Settings;
using FLS.Server.Tests.Infrastructure.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLS.Server.Tests.WebApiControllerTests
{
    [TestClass]
    public class SettingsControllerTest : BaseAuthenticatedTests
    {
        [TestMethod]
        [TestCategory("WebApi")]
        public void GetSettingsControllerTest()
        {
            var settingDetails = new SettingDetails()
            {
                UserId = CurrentIdentityUser.UserId,
                SettingKey = "Language",
                SettingValue = "DE"
            };

            var response = PostAsync(settingDetails, Uri).Result;

            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            var responseDetails = ConvertToModel<SettingDetails>(response);
            Assert.IsNotNull(responseDetails);

            var filter = new SettingDetailsSearchFilter()
            {
                UserId = settingDetails.UserId,
                SettingKey = settingDetails.SettingKey
            };

            var response1 = PostAsync(filter, Uri + "/key").Result;
            var responseDetails1 = ConvertToModel<string>(response1);
            Assert.IsNotNull(responseDetails1);

            Assert.AreEqual(settingDetails.SettingValue, responseDetails1);
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void InsertOrUpdateSettingsControllerTest()
        {
            var settingDetails = new SettingDetails()
            {
                ClubId = CurrentIdentityUser.ClubId,
                SettingKey = "ClubKey1",
                SettingValue = "Bla"
            };

            var response = PostAsync(settingDetails, Uri).Result;

            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            var responseDetails = ConvertToModel<SettingDetails>(response);
            Assert.IsNotNull(responseDetails);

            var filter = new SettingDetailsSearchFilter()
            {
                ClubId = settingDetails.ClubId,
                SettingKey = settingDetails.SettingKey
            };

            var response1 = PostAsync(filter, Uri + "/key").Result;
            var responseDetails1 = ConvertToModel<string>(response1);
            Assert.IsNotNull(responseDetails1);

            Assert.AreEqual(settingDetails.SettingValue, responseDetails1);


            var settingDetailsUpdated = new SettingDetails()
            {
                ClubId = CurrentIdentityUser.ClubId,
                SettingKey = "ClubKey1",
                SettingValue = "BlaBla-Update"
            };

            var responseUpdated = PostAsync(settingDetailsUpdated, Uri).Result;

            Assert.IsTrue(responseUpdated.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", responseUpdated.StatusCode));
            var responseDetailsUpdated = ConvertToModel<SettingDetails>(responseUpdated);
            Assert.IsNotNull(responseDetailsUpdated);

            var response2 = PostAsync(filter, Uri + "/key").Result;
            var responseDetails2 = ConvertToModel<string>(response2);
            Assert.IsNotNull(responseDetails2);

            Assert.AreEqual(responseDetailsUpdated.SettingValue, responseDetails2);
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void DeleteSettingsControllerTest()
        {
            var settingDetails = new SettingDetails()
            {
                UserId = CurrentIdentityUser.UserId,
                SettingKey = "KeyToDelete",
                SettingValue = "Must be deleted"
            };

            var response = PostAsync(settingDetails, Uri).Result;

            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            var responseDetails = ConvertToModel<SettingDetails>(response);
            Assert.IsNotNull(responseDetails);

            var filter = new SettingDetailsSearchFilter()
            {
                UserId = settingDetails.UserId,
                SettingKey = settingDetails.SettingKey
            };

            var response1 = DeletePostAsync(filter, Uri).Result;

            Assert.IsTrue(response1.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response1.StatusCode));
        }

        protected override string Uri
        {
            get { return RoutePrefix; }
        }

        protected override string RoutePrefix
        {
            get { return "/api/v1/settings"; }
        }
    }
}
