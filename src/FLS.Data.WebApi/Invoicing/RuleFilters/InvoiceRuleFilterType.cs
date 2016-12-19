namespace FLS.Data.WebApi.Invoicing.RuleFilters
{
    public enum InvoiceRuleFilterType
    {
        RecipientInvoiceRuleFilter = 1,
        AircraftInvoiceRuleFilter = 2,
        AdditionalFuelFeeInvoiceRuleFilter = 3,
        InstructorFeeInvoiceRuleFilter = 4,
        LandingTaxInvoiceRuleFilter = 5,
        NoLandingTaxInvoiceRuleFilter = 6,
        VsfFeeInvoiceRuleFilter = 7
    };
}