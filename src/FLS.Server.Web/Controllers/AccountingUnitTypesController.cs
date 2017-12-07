using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using FLS.Data.WebApi;
using FLS.Data.WebApi.Accounting;
using FLS.Data.WebApi.Resources;
using FLS.Server.Service.Accounting;

namespace FLS.Server.WebApi.Controllers
{
    /// <summary>
    /// Api controller for accountingUnitType entities.
    /// </summary>
    [Authorize(Roles = RoleApplicationKeyStrings.ClubAdministrator)]
    [RoutePrefix("api/v1/accountingunittypes")]
    public class AccountingUnitTypesController : ApiController
    {
        private readonly AccountingRuleService _accountingRuleService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountingUnitTypesController"/> class.
        /// </summary>
        public AccountingUnitTypesController(AccountingRuleService accountingRuleService)
        {
            _accountingRuleService = accountingRuleService;
        }

        /// <summary>
        /// Gets the accounting rule filter types list items.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(List<AccountingUnitTypeListItem>))]
        public IHttpActionResult GetAccountingUnitTypeListItems()
        {
            var items = _accountingRuleService.GetAccountingUnitTypeListItems();
            return Ok(items);
        }
        
    }
}
