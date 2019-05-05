using System.Linq;
using FLS.Data.WebApi;
using FLS.Data.WebApi.Accounting.RuleFilters;
using FLS.Server.Tests.Infrastructure.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLS.Server.Tests.WebApiControllerTests
{
    [TestClass]
    public class AccountingRuleFiltersControllerTest : BaseAuthenticatedTests
    {
        [TestMethod]
        [TestCategory("WebApi")]
        public void GetPagedAccountingRuleFilterOverviewWebApiTest()
        {
            var pageSize = 100;
            var searchFilter = new PageableSearchFilter<AccountingRuleFilterOverviewSearchFilter>();
            searchFilter.SearchFilter = new AccountingRuleFilterOverviewSearchFilter();
            searchFilter.Sorting.Add("SortIndicator", "asc");
            searchFilter.SearchFilter.Target = "1061";
            searchFilter.SearchFilter.RuleFilterName = "Schulung";

            var response = PostAsync(searchFilter, $"/api/v1/accountingrulefilters/page/0/{pageSize}").Result;

            var result = ConvertToModel<PagedList<AccountingRuleFilterOverview>>(response);

            Assert.IsTrue(result.Items.Any(), "No items found!");

            result.Items.ForEach(p => Logger.Debug($"{p.ToString()}"));
        }
        
        protected override string Uri
        {
            get { return RoutePrefix; }
        }

        protected override string RoutePrefix
        {
            get { return "/api/v1/locations"; }
        }
    }
}
