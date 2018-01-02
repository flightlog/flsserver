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
        public void SetSettingServiceTest()
        {
            var id = Guid.NewGuid();

            var json = JsonConvert.SerializeObject(id);

            var settingDetails = new SettingDetails()
            {
                ClubId = CurrentIdentityUser.ClubId,
                SettingKey = SettingKey.TrialFlightAircraftReservationFlightTypeId,
                SettingValue = json
            };

            SettingService.InsertSettingDetails(settingDetails);

            var setting = SettingService.GetSettingValue(settingDetails.SettingKey, settingDetails.ClubId,
                settingDetails.UserId);

        }
    }
}
