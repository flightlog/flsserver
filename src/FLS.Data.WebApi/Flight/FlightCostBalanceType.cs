namespace FLS.Data.WebApi.Flight
{
    /// <summary>
    /// Unit type for the cost center. For more details <see cref="http://wiki.glider-fls.ch/index.php?title=FlightTypeCostCenters-Tabelle"/>
    /// </summary>
    public enum FlightCostBalanceType
    {
        PilotPaysAllCosts = 1,
        HalfHalfPayment = 2,
        TowPilotTakesHisCosts = 3,
        NoInstructorFee = 4,
        CostsPaidByPerson = 5
    }
}