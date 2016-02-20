using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using FLS.Data.WebApi.PlanningDay;
using FLS.Server.Service;

namespace FLS.Server.WebApi.Controllers
{
    /// <summary>
    /// Api controller for planning day entities.
    /// </summary>
    [Authorize]
    [RoutePrefix("api/v1/planningdays")]
    public class PlanningDaysController : ApiController
    {
        private readonly PlanningDayService _planningDayService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlanningDaysController"/> class.
        /// </summary>
        public PlanningDaysController(PlanningDayService planningDayService)
        {
            _planningDayService = planningDayService;
        }

        /// <summary>
        /// Gets the planning day overviews.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [Route("overview")]
        [ResponseType(typeof(List<PlanningDayOverview>))]
        public IHttpActionResult GetPlanningDayOverviews()
        {
            var planningDays = _planningDayService.GetPlanningDayOverview();
            return Ok(planningDays);
        }

        /// <summary>
        /// Gets the planning day overviews.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("future")]
        [Route("overview/future")]
        [ResponseType(typeof(List<PlanningDayOverview>))]
        public IHttpActionResult GetFuturePlanningDayOverviews()
        {
            var planningDays = _planningDayService.GetPlanningDayOverview(DateTime.Now.Date);
            return Ok(planningDays);
        }

        /// <summary>
        /// Gets the planning day overviews.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("fromdate/{fromDate:datetime:regex(\\d{4}-\\d{2}-\\d{2})}")]
        [ResponseType(typeof(List<PlanningDayOverview>))]
        public IHttpActionResult GetPlanningDayOverviews(DateTime fromDate)
        {
            var planningDays = _planningDayService.GetPlanningDayOverview(fromDate);
            return Ok(planningDays);
        }

        /// <summary>
        /// Gets the planning day details.
        /// </summary>
        /// <param name="planningDayId">The planning day identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{planningDayId}")]
        [ResponseType(typeof(PlanningDayDetails))]
        public IHttpActionResult GetPlanningDayDetails(Guid planningDayId)
        {
            var details = _planningDayService.GetPlanningDayDetails(planningDayId);
            return Ok(details);
        }

        /// <summary>
        /// Creates the planning days based on the creator rule.
        /// </summary>
        /// <param name="planningDayCreatorRule">The planning day creator rule.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("create/rule")]
        [ResponseType(typeof(List<PlanningDayOverview>))]
        public IHttpActionResult CreatePlanningDays([FromBody] PlanningDayCreatorRule planningDayCreatorRule)
        {
            var planningDayOverviews = _planningDayService.CreatePlanningDays(planningDayCreatorRule);
            return Ok(planningDayOverviews);
        }

        /// <summary>
        /// Creates the planning days.
        /// </summary>
        /// <param name="planningDays">The planning days.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("create/list")]
        [ResponseType(typeof(List<PlanningDayDetails>))]
        public IHttpActionResult CreatePlanningDays([FromBody] List<PlanningDayDetails> planningDays)
        {
            var updatedPlanningDays = _planningDayService.CreatePlanningDays(planningDays);
            return Ok(updatedPlanningDays);
        }

        /// <summary>
        /// Inserts the specified planning day details.
        /// </summary>
        /// <param name="planningDayDetails">The planning day details.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(PlanningDayDetails))]
        public IHttpActionResult Insert([FromBody] PlanningDayDetails planningDayDetails)
        {
            _planningDayService.InsertPlanningDayDetails(planningDayDetails);
            return Ok(planningDayDetails);
        }

        /// <summary>
        /// Updates the specified planning day.
        /// </summary>
        /// <param name="planningDayId">The planning day identifier.</param>
        /// <param name="planningDayDetails">The planning day details.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{planningDayId}")]
        [ResponseType(typeof(PlanningDayDetails))]
        public IHttpActionResult Update(Guid planningDayId, [FromBody]PlanningDayDetails planningDayDetails)
        {
            _planningDayService.UpdatePlanningDayDetails(planningDayDetails);
            return Ok(planningDayDetails);
        }

        /// <summary>
        /// Deletes the specified planning day.
        /// </summary>
        /// <param name="planningDayId">The planning day identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{planningDayId}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult Delete(Guid planningDayId)
        {
            _planningDayService.DeletePlanningDayDetails(planningDayId);
            return Ok();
        }
    }
}
