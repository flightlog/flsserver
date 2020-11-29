using System;
using System.Collections.Generic;
using System.Linq;
using FLS.Common.Extensions;
using FLS.Data.WebApi;
using FLS.Data.WebApi.Flight;
using FLS.Data.WebApi.Reporting.Flights;
using FLS.Server.Data.Enums;
using FLS.Server.Tests.Infrastructure.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLS.Server.Tests.WebApiControllerTests
{
    [TestClass]
    public class FlightReportsControllerTest : BaseAuthenticatedTests
    {
        [TestMethod]
        [TestCategory("WebApi")]
        public void FlightExcelExportWebApiTest()
        {
            var pageSize = 2000;
            var searchFilter = new PageableSearchFilter<FlightReportFilterCriteria>();
            searchFilter.SearchFilter = new FlightReportFilterCriteria();
            searchFilter.SearchFilter.FlightDate = new DateTimeFilter()
            {
                From = new DateTime(2019, 1, 1),
                To = new DateTime(2019, 12, 31)
            };
            searchFilter.SearchFilter.GliderFlights = true;
            

            var response = PostAsync<PageableSearchFilter<FlightReportFilterCriteria>>(searchFilter, $"/api/v1/flightreports/export/excel/1/{pageSize}").Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
            //var result = ConvertToModel<PagedList<LocationOverview>>(response);

            //Assert.AreEqual(pageSize, result.Items.Count, 0, "PageSize does not fit with items count in list.");
        }

        protected override string Uri
        {
            get { return RoutePrefix; }
        }

        protected override string RoutePrefix
        {
            get { return "/api/v1/flights"; }
        }
    }
}
