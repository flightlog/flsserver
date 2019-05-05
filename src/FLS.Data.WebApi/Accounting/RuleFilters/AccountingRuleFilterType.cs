namespace FLS.Data.WebApi.Accounting.RuleFilters
{
    public enum AccountingRuleFilterType
    {
        RecipientAccountingRuleFilter = 10,
        NoLandingTaxAccountingRuleFilter = 20,
        FlightTimeAccountingRuleFilter = 30,
        InstructorFeeAccountingRuleFilter = 40,
        AdditionalFuelFeeAccountingRuleFilter = 50,
        StartTaxAccountingRuleFilter = 55,
        LandingTaxAccountingRuleFilter = 60,
        VsfFeeAccountingRuleFilter = 70,
        EngineTimeAccountingRuleFilter = 80
    };
}