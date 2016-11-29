namespace FLS.Data.WebApi.Invoicing.RuleFilters
{
    public class InvoiceRecipientRuleFilter : BaseRuleFilter
    {
        public InvoiceRecipientRuleFilter()
        {
            UseRuleForAllStartLocationsExceptListed = true;
            UseRuleForAllLdgLocationsExceptListed = true;
            UseRuleForAllFlightCrewTypesExceptListed = true;
            UseRuleForAllAircraftsExceptListed = true;
            UseRuleForAllClubMemberNumbersExceptListed = true;
            UseRuleForAllFlightTypesExceptListed = true;
        }
        public bool IsInvoicedToClubInternal { get; set; }
    }
}
