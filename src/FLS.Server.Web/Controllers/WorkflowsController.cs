using System.Web.Http;
using System.Web.Http.Description;
using FLS.Server.Service;

namespace FLS.Server.WebApi.Controllers
{
    /// <summary>
    /// Api controller for workflows.
    /// </summary>
    [Authorize]
    [RoutePrefix("api/v1/workflows")]
//    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class WorkflowsController : ApiController
    {
        private readonly WorkflowService _workflowService;
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowsController"/> class.
        /// </summary>
        public WorkflowsController(WorkflowService workflowService)
        {
            _workflowService = workflowService;
        }

        /// <summary>
        /// Executes all workflows which needs to be executed
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(void))]
        public IHttpActionResult ExecuteWorkflows()
        {
            _workflowService.ExecuteWorkflows();
            return Ok();
        }

        /// <summary>
        /// Executes the daily report job.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("dailyreports")]
        [ResponseType(typeof(void))]
        public IHttpActionResult ExecuteDailyReportJob()
        {
            _workflowService.ExecuteDailyReportJob();
            return Ok();
        }

        /// <summary>
        /// Executes the monthly report job.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("monthlyreports")]
        [ResponseType(typeof(void))]
        public IHttpActionResult ExecuteMonthlyReportJob()
        {
            _workflowService.ExecuteAircraftStatisticReportJob();
            return Ok();
        }

        /// <summary>
        /// Executes the monthly report job.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("monthlyreports/{year:int}/{month:int}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult ExecuteMonthlyReportJob(int year, int month)
        {
            _workflowService.ExecuteAircraftStatisticReportJob(year, month);
            return Ok();
        }

        /// <summary>
        /// Executes the flight validation.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("flightvalidation")]
        [ResponseType(typeof(void))]
        public IHttpActionResult ExecuteFlightValidation()
        {
            _workflowService.ExecuteDailyFlightValidationJob();
            return Ok();
        }

        /// <summary>
        /// Executes the planning day mails.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("planningdaymails")]
        [ResponseType(typeof(void))]
        public IHttpActionResult ExecutePlanningDayMails()
        {
            _workflowService.ExecutePlanningDayMailJob();
            return Ok();
        }

        /// <summary>
        /// Executes the test mail.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("testmails")]
        [ResponseType(typeof(void))]
        public IHttpActionResult ExecuteTestMail()
        {
            _workflowService.ExecuteTestMailJob();
            return Ok();
        }

        /// <summary>
        /// Executes the delivery creation job.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("deliverycreation")]
        [ResponseType(typeof(void))]
        public IHttpActionResult ExecuteDeliveryCreationJob()
        {
            _workflowService.ExecuteDeliveryCreationJob();
            return Ok();
        }

        /// <summary>
        /// Executes the delivery mail export job.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("deliverymailexport")]
        [ResponseType(typeof(void))]
        public IHttpActionResult ExecuteDeliveryMailExportJob()
        {
            _workflowService.ExecuteDeliveryMailExportJob();
            return Ok();
        }
    }
}
