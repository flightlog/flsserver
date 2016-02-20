using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using FLS.Data.WebApi.Emails;
using FLS.Data.WebApi.Resources;
using FLS.Server.Service;

namespace FLS.Server.WebApi.Controllers
{
    /// <summary>
    /// Api controller for emailTemplate entities.
    /// </summary>
    [Authorize(Roles = RoleApplicationKeyStrings.ClubAdministrator + "," + RoleApplicationKeyStrings.SystemAdministrator)]
    [RoutePrefix("api/v1/emailtemplates")]
    public class EmailTemplatesController : ApiController
    {
        private readonly TemplateService _templateService;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailTemplatesController"/> class.
        /// </summary>
        public EmailTemplatesController(TemplateService templateService)
        {
            _templateService = templateService;
        }

        /// <summary>
        /// Gets the emailTemplate overviews.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [Route("overview")]
        [ResponseType(typeof(List<EmailTemplateOverview>))]
        public IHttpActionResult GetEmailTemplateOverviews()
        {
            var emailTemplates = _templateService.GetEmailTemplateOverviews();
            return Ok(emailTemplates);
        }
        
        /// <summary>
        /// Gets the emailTemplate details.
        /// </summary>
        /// <param name="emailTemplateId">The emailTemplate identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{emailTemplateId}")]
        [ResponseType(typeof(EmailTemplateDetails))]
        public IHttpActionResult GetEmailTemplateDetails(Guid emailTemplateId)
        {
            var emailTemplateDetails = _templateService.GetEmailTemplateDetails(emailTemplateId);
            return Ok(emailTemplateDetails);
        }

        /// <summary>
        /// Inserts the specified emailTemplate details.
        /// </summary>
        /// <param name="emailTemplateDetails">The emailTemplate details.</param>
        /// <returns></returns>
        [Authorize(Roles = RoleApplicationKeyStrings.SystemAdministrator)]
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(EmailTemplateDetails))]
        public IHttpActionResult Insert([FromBody] EmailTemplateDetails emailTemplateDetails)
        {
            _templateService.InsertEmailTemplateDetails(emailTemplateDetails);
            return Ok(emailTemplateDetails);
        }

        /// <summary>
        /// Updates the specified emailTemplate.
        /// </summary>
        /// <param name="emailTemplateId">The emailTemplate identifier.</param>
        /// <param name="emailTemplateDetails">The emailTemplate details.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{emailTemplateId}")]
        [ResponseType(typeof(EmailTemplateDetails))]
        public IHttpActionResult Update(Guid emailTemplateId, [FromBody]EmailTemplateDetails emailTemplateDetails)
        {
            _templateService.UpdateEmailTemplateDetails(emailTemplateDetails);
            return Ok(emailTemplateDetails);
        }

        /// <summary>
        /// Deletes the specified emailTemplate.
        /// </summary>
        /// <param name="emailTemplateId">The emailTemplate identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{emailTemplateId}")]
        public IHttpActionResult Delete(Guid emailTemplateId)
        {
            _templateService.DeleteEmailTemplate(emailTemplateId);
            return Ok();
        }
    }
}
