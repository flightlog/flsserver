using System.Linq;
using FLS.Data.WebApi;
using FLS.Data.WebApi.Reporting.Flights;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLS.Server.Tests.ServiceTests
{
    [TestClass]
    public class FlightReportServiceTest : BaseTest
    {

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void FlightReportServiceTestInitialize(TestContext testContext)
        {
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
        public void GetFlightReportsTest()
        {
            Assert.IsNotNull(IdentityService);
            Assert.IsNotNull(IdentityService.CurrentAuthenticatedFLSUser);

            var pilots = PersonService.GetGliderPilotPersonListItems(true);

            Assert.IsTrue(pilots.Any());
            Flight flight = null;

            using (var context = DataAccessService.CreateDbContext())
            {
                foreach (var pilot in pilots)
                {
                    var flights = context.Flights.Include(Constants.FlightCrews)
                        .Where(x => x.FlightCrews.Any(fc => fc.PersonId == pilot.PersonId && fc.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.PilotOrStudent))
                        .Where(q => q.ProcessStateId != (int)FLS.Data.WebApi.Flight.FlightProcessState.Invalid);

                    if (flights.Any())
                    {
                        flight = flights.First();

                        var flightDate = flight.FlightDate.Value;

                        var filter = new FlightReportFilterCriteria()
                        {
                            GliderFlights = true,
                            MotorFlights = false,
                            FlightCrewPersonId = flight.Pilot.PersonId,
                            LocationId = flight.StartLocationId.GetValueOrDefault(),
                            FlightDate = new DateTimeFilter()
                            {
                                From = flightDate.Date,
                                To = flightDate.Date.AddDays(1)
                            }
                        };

                        var pageableFilter = new PageableSearchFilter<FlightReportFilterCriteria>()
                        {
                            SearchFilter = filter
                        };

                        var result = FlightReportService.GetPagedFlightReport(0, 50, pageableFilter);
                        Assert.IsNotNull(result);
                    }
                }
            }
        }
    }
}
