using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using FLS.Data.WebApi.Accounting.RuleFilters;
using FLS.Data.WebApi.Resources;
using FLS.Server.Service.Accounting;

namespace FLS.Server.WebApi.Controllers
{
    /// <summary>
    /// Api controller for accountingRule entities.
    /// </summary>
    [Authorize(Roles = RoleApplicationKeyStrings.ClubAdministrator)]
    [RoutePrefix("api/v1/accountingrulefilters")]
    public class AccountingRuleFiltersController : ApiController
    {
        private readonly AccountingRuleService _accountingRuleService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountingRuleFiltersController"/> class.
        /// </summary>
        public AccountingRuleFiltersController(AccountingRuleService accountingRuleService)
        {
            _accountingRuleService = accountingRuleService;
        }

        /// <summary>
        /// Gets the accounting rule filter overviews.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [Route("overview")]
        [ResponseType(typeof(List<AccountingRuleFilterOverview>))]
        public IHttpActionResult GetAccountingRuleFilterOverviews()
        {
            var accountingRuleFilters = _accountingRuleService.GetAccountingRuleFilterOverview();
            return Ok(accountingRuleFilters);
        }

        /// <summary>
        /// Gets the accounting rule filter details.
        /// </summary>
        /// <param name="accountingRuleFilterId">The accounting rule filter identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{accountingRuleFilterId}")]
        [ResponseType(typeof(AccountingRuleFilterDetails))]
        public IHttpActionResult GetAccountingRuleFilterDetails(Guid accountingRuleFilterId)
        {
            var accountingRuleDetails = _accountingRuleService.GetAccountingRuleFilterDetails(accountingRuleFilterId);
            return Ok(accountingRuleDetails);
        }

        /// <summary>
        /// Inserts the specified accounting rule filter details.
        /// </summary>
        /// <param name="accountingRuleFilterDetails">The accounting rule filter details.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(AccountingRuleFilterDetails))]
        public IHttpActionResult Insert([FromBody] AccountingRuleFilterDetails accountingRuleFilterDetails)
        {
            _accountingRuleService.InsertAccountingRuleFilterDetails(accountingRuleFilterDetails);
            return Ok(accountingRuleFilterDetails);
        }

        /// <summary>
        /// Updates the specified accounting rule filter identifier.
        /// </summary>
        /// <param name="accountingRuleFilterId">The accounting rule filter identifier.</param>
        /// <param name="accountingRuleFilterDetails">The accounting rule filter details.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{accountingRuleFilterId}")]
        [ResponseType(typeof(AccountingRuleFilterDetails))]
        public IHttpActionResult Update(Guid accountingRuleFilterId, [FromBody]AccountingRuleFilterDetails accountingRuleFilterDetails)
        {
            _accountingRuleService.UpdateAccountingRuleFilterDetails(accountingRuleFilterDetails);
            return Ok(accountingRuleFilterDetails);
        }

        /// <summary>
        /// Deletes the specified accounting rule filter identifier.
        /// </summary>
        /// <param name="accountingRuleFilterId">The accounting rule filter identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{accountingRuleFilterId}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult Delete(Guid accountingRuleFilterId)
        {
            _accountingRuleService.DeleteAccountingRuleFilter(accountingRuleFilterId);
            return Ok();
        }
        
    }
}
