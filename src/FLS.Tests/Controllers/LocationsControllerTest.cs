using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using FLS.Data.WebApi.Location;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FLS;
using FLS.Controllers;

namespace FLS.Tests.Controllers
{
    [TestClass]
    public class LocationsControllerTest
    {
        [TestMethod]
        public void GetLocationOverviewTest()
        {
            // Arrange
            ValuesController controller = new ValuesController();

            // Act
            IEnumerable<string> result = controller.Get();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("value1", result.ElementAt(0));
            Assert.AreEqual("value2", result.ElementAt(1));
        }

        [TestMethod]
        public void GetLocationDetailsTest()
        {
            // Arrange
            LocationsController controller = new LocationsController();

            var id = controller.GetLocationOverviews().First().LocationId;
            // Act
            IHttpActionResult result = controller.GetLocationDetails(id);
            
            // Assert
            Assert.AreEqual("value", result);
        }

        [TestMethod]
        public void PostLocationDetailsTest()
        {
            // Arrange
            LocationsController controller = new LocationsController();

            var location = new LocationDetails();
            location.LocationName = "Location @ " + DateTime.Now.ToShortTimeString();
            location.CountryId = Guid.Parse("77CC3BE6-95DB-11E0-B104-E7F04724019B"); //CH
            location.LocationTypeId = Guid.Parse("82F6B213-1C96-4FF0-BD29-CCE262163DB9");
            Assert.AreEqual(location.Id, Guid.Empty);

            // Act
            controller.PostLocationDetails(location);

            // Assert
        }

        [TestMethod]
        public void PutLocationDetailsTest()
        {
            // Arrange
            ValuesController controller = new ValuesController();

            // Act
            controller.Put(5, "value");

            // Assert
        }

        [TestMethod]
        public void Delete()
        {
            // Arrange
            ValuesController controller = new ValuesController();

            // Act
            controller.Delete(5);

            // Assert
        }
    }
}
