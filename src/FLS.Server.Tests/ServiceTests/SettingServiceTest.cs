using System;
using System.Linq;
using FLS.Data.WebApi.Settings;
using FLS.Server.Data.Resources;
using FLS.Server.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;

namespace FLS.Server.Tests.ServiceTests
{
    [TestClass]
    public class SettingServiceTest : BaseTest
    {
        [TestMethod]
        [TestCategory("Service")]
        public void InsertOrUpdateSettingDetailsTest()
        {
            var id = Guid.NewGuid();

            var json = JsonConvert.SerializeObject(id);
            var settingDetails = new SettingDetails()
            {
                ClubId = CurrentIdentityUser.ClubId,
                SettingKey = $"Test_{DateTime.Now.Ticks}",
                SettingValue = json
            };

            SettingService.InsertOrUpdateSettingDetails(settingDetails);

            var setting = SettingService.GetSettingValue(settingDetails.SettingKey, settingDetails.ClubId,
                settingDetails.UserId);

            Assert.IsFalse(string.IsNullOrWhiteSpace(setting));

            var settingValue = JsonConvert.DeserializeObject<Guid>(setting);

            Assert.AreEqual(id, settingValue);

            var newValue = "TestValue";

            settingDetails.SettingValue = newValue;

            SettingService.InsertOrUpdateSettingDetails(settingDetails);

            var settingValue2 = SettingService.GetSettingValue(settingDetails.SettingKey, settingDetails.ClubId,
                settingDetails.UserId);

            Assert.IsFalse(string.IsNullOrWhiteSpace(settingValue2));

            Assert.AreEqual(newValue, settingValue2);
        }

        [TestMethod]
        [TestCategory("Service")]
        public void TryGetSettingsValueTest()
        {
            bool useClubPlanningDayWithoutReservations;
            SettingService.TryGetSettingValue(SettingKey.ClubUsePlanningDayWithoutReservations, CurrentIdentityUser.ClubId, null, out useClubPlanningDayWithoutReservations);

            Assert.IsFalse(useClubPlanningDayWithoutReservations);

            var settingDetails = new SettingDetails()
            {
                ClubId = CurrentIdentityUser.ClubId,
                SettingKey = SettingKey.ClubUsePlanningDayWithoutReservations,
                SettingValue = JsonConvert.SerializeObject(true)
            };

            SettingService.InsertOrUpdateSettingDetails(settingDetails);

            SettingService.TryGetSettingValue(SettingKey.ClubUsePlanningDayWithoutReservations, CurrentIdentityUser.ClubId, null, out useClubPlanningDayWithoutReservations);

            Assert.IsTrue(useClubPlanningDayWithoutReservations);
        }
    }
}
