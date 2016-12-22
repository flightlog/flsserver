namespace FLS.Data.WebApi.Invoicing.RuleFilters
{
    public enum InvoiceRuleFilterType
    {
        RecipientInvoiceRuleFilter = 10,
        NoLandingTaxInvoiceRuleFilter = 20,
        AircraftInvoiceRuleFilter = 30,
        InstructorFeeInvoiceRuleFilter = 40,
        AdditionalFuelFeeInvoiceRuleFilter = 50,
        LandingTaxInvoiceRuleFilter = 60,
        VsfFeeInvoiceRuleFilter = 70
    };
}