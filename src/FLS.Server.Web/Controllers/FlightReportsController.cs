using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Description;
using FLS.Data.WebApi;
using FLS.Data.WebApi.Flight;
using FLS.Data.WebApi.Processing;
using FLS.Data.WebApi.Reporting.Flights;
using FLS.Data.WebApi.Resources;
using FLS.Server.Service;
using FLS.Server.Service.Reporting;
using FLS.Server.WebApi.ActionFilters;

namespace FLS.Server.WebApi.Controllers
{
    /// <summary>
    /// Api controller for flight reports
    /// </summary>
    [Authorize]
    [RoutePrefix("api/v1/flightreports")]
    public class FlightReportsController : ApiController
    {
        private readonly FlightReportService _flightReportService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlightReportsController"/> class.
        /// </summary>
        public FlightReportsController(FlightReportService flightReportService)
        {
            _flightReportService = flightReportService;
        }

        /// <summary>
        /// Gets the flight reports.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("page/{pageStart:int?}/{pageSize:int?}")]
        [ResponseType(typeof(FlightReportResult))]
        public IHttpActionResult GetPagedFlightReport([FromBody]PageableSearchFilter<FlightReportFilterCriteria> pageableSearchFilter, int? pageStart = 0, int? pageSize = 100)
        {
            var result = _flightReportService.GetPagedFlightReport(pageStart, pageSize, pageableSearchFilter);
            return Ok(result);
        }

        /// <summary>
        /// Gets the flight reports as excel.
        /// </summary>
        /// <seealso cref="https://stackoverflow.com/questions/9541351/returning-binary-file-from-controller-in-asp-net-web-api"/>
        /// <returns></returns>
        [HttpPost]
        [Route("export/excel/{pageStart:int?}/{pageSize:int?}")]
        [ResponseType(typeof(FlightReportResult))]
        public IHttpActionResult GetExcelFlightReport([FromBody] PageableSearchFilter<FlightReportFilterCriteria> pageableSearchFilter, int? pageStart = 0, int? pageSize = 100)
        {
            var report = _flightReportService.ExportExcel(pageStart, pageSize, pageableSearchFilter);

#if DEBUG
            var filename = $"FlightReport-{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.xlsx";

            var fullfilename = @"C:\temp\" + filename;

            System.IO.File.WriteAllBytes(fullfilename, report);

#endif
            var response = new HttpResponseMessage();
            response.Content = new ByteArrayContent(report);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.ms-excel");

            return Ok(response);
        }
    }
}
