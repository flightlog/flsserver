using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using FLS.Data.WebApi;
using FLS.Data.WebApi.Accounting.RuleFilters;
using FLS.Data.WebApi.Resources;
using FLS.Server.Service.Accounting;

namespace FLS.Server.WebApi.Controllers
{
    /// <summary>
    /// Api controller for accountingRuleFilterType entities.
    /// </summary>
    [Authorize(Roles = RoleApplicationKeyStrings.ClubAdministrator)]
    [RoutePrefix("api/v1/accountingrulefiltertypes")]
    public class AccountingRuleFilterTypesController : ApiController
    {
        private readonly AccountingRuleService _accountingRuleService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountingRuleFiltersController"/> class.
        /// </summary>
        public AccountingRuleFilterTypesController(AccountingRuleService accountingRuleService)
        {
            _accountingRuleService = accountingRuleService;
        }

        /// <summary>
        /// Gets the accounting rule filter types list items.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(List<AccountingRuleFilterTypeListItem>))]
        public IHttpActionResult GetAccountingRuleFilterTypeListItems()
        {
            var items = _accountingRuleService.GetAccountingRuleFilterTypeListItems();
            return Ok(items);
        }
        
    }
}
