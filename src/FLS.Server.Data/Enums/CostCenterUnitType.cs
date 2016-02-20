namespace FLS.Server.Data.Enums
{
    /// <summary>
    /// Unit type for the cost center. For more details <see cref="http://wiki.glider-fls.ch/index.php?title=FlightTypeCostCenters-Tabelle"/>
    /// </summary>
    public enum CostCenterUnitType
    {
        PerFlightMinute = 0,
        PerStartOrFlight = 1,
        PerLanding = 2
    }
}